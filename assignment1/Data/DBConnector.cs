using System.Collections;
using System.Data;
using assignment1.Libs;
using assignment1.Models;
using assignment1.Models.Auction;
using assignment1.Models.Auth;
using assignment1.Models.Generics;
using Data;

namespace assignment1.Data
{
    public class DBConnector : MainData
    {
        public DBConnector() : base(System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(Persistent.CnxString)))
        {
        }

        public bool NewUser(RegistrationModel _register)
        {
            var model = GetDataModel("UserType");
            model.Rows.Add(_register.ToDataRow(model));
            int affected = this.NonQueryExecuteSQL("[brb_CRUD_user]", new Hashtable() { { "@table", model }, { "@type", CRUD.Create } });
            return affected > 0;
        }

        public object LoginUser(LoginModel _login)
        {
            DataTable info = this.GetSQLData("[brb_login]", new Hashtable() { { "@username", _login.Username } }); //, { "@password", _login.Password }
            if (info.Rows.Count <= 0) return "No user with that credentials was found! 🤨";
            else if (info.Rows.Count >= 2) return "Something is not quite right 🤔";
            else return new LoginModel(info.Rows[0]);
        }

        public object RecoverSession(string _session)
        {
            DataTable info = this.GetSQLData("[brb_recover_session]", new Hashtable() { { "@session", _session } });
            if (info.Rows.Count == 1) return new LoginModel(info.Rows[0]);
            else return "Something is not quite right 🤔"; // false; // 
        }

        public IEnumerable<MenuModel> GetMenus(int _user_type_id)
        {
            DataTable info = this.GetSQLData("[brb_get_menus]", new Hashtable() { { "@user_type_id", _user_type_id } });
            foreach (DataRow row in info.Rows)
                yield return new MenuModel(row);
        }

        public AuctionModel GetAuction(int _auction_id)
        {
            DataSet info = this.GetMultiTables("[brb_get_auction]", new Hashtable { { "@auction_id", _auction_id } });
            if (info.Tables[0].Rows.Count == 0) return new AuctionModel();
            return new AuctionModel(info.Tables[0].Rows[0], info.Tables[1]);
        }

        public enum LandingPageAuctionsOptions : byte { Carrousel, Last50, Last100 }
        public IEnumerable<AuctionModel> GetLastAuctions(LandingPageAuctionsOptions _option)
        {
            DataTable table = this.GetSQLData("[brb_get_last_auctions]", new Hashtable() { { "@option", (byte)_option } });
            foreach (DataRow row in table.Rows)
                yield return new AuctionModel(row);
        }

        public IEnumerable<AuctionModel> SearchAuctions(string _search)
        {
            // Option 1 - Local Linear Search
            // IEnumerable<AuctionModel> data = this.GetLastAuctions(LandingPageAuctionsOptions.Last100);
            // return data.Where(x =>
            // {
            //     if (_search.Contains(',')) // that would work as an OR, but there could be more, and mixed operators, like & for AND. And () for more complex searches.
            //     {
            //         string[] terms = _search.Split(',');
            //         return terms.Any(t =>
            //         {
            //             if (!t.Contains(':')) return false;
            //             string[] prop = t.Split(':');
            //             return x.GetType().GetProperty(prop[0], System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(x).ToString().Equals(prop[1]);
            //         });
            //     }

            //     return x.Auction_name.Contains(_search);
            // });

            // Option 2 - Database Search
            DataTable data = this.GetSQLData("[brb_auction_search]", new Hashtable() { { "@search", _search } });
            foreach (DataRow row in data.Rows)
                yield return new AuctionModel(row);

            /*
            Hi :) Bruno here, I found interesting the local search of the last 100 auction items, however, that leave the
                algorithm just to a recent search and return the whole table and then filtered locally is not an option.
            So, I believe that the best approach would be to send the search info to the database and work the search algorithm on SQL,
                that would leave the code here significantly shorter and all the searching power will occur in the database which have 
                to its disposition all the tables.
            All the database scripts are in the "database.sql" file
            */
        }

        public bool AddBid(BidModel _bid)
        {
            var model = GetDataModel("BidType");
            model.Rows.Add(_bid.ToDataRow(model));
            int affected = this.NonQueryExecuteSQL("[brb_CRUD_bid]", new Hashtable() { { "@table", model }, { "@type", CRUD.Create } });
            return affected > 0;
        }

        public IEnumerable<dynamic> GetUserTypes()
        {
            DataTable data = this.GetSQLData("[brb_get_user_types]");
            foreach (DataRow row in data.Rows)
                yield return new { UserTypeId = row["user_type_id"], UserTypeName = row["user_type_name"] };
        }

        public bool UpdateUser(UserBase _user)
        {
            var model = GetDataModel("UserType");
            model.Rows.Add(_user.ToDataRow(model));
            int affected = this.NonQueryExecuteSQL("[brb_CRUD_user]", new Hashtable() { { "@table", model }, { "@type", CRUD.Update } });
            return affected > 0;
        }

        public bool SetSession(string _username, string _session)
        {
            return this.NonQueryExecuteSQL("[brb_set_session]", new Hashtable() { { "@username", _username }, { "@session", _session } }) > 0;
        }

        public bool NewAuction(AuctionModel _auction)
        {
            var model = this.GetDataModel("AuctionType");
            model.Rows.Add(_auction.ToDataRow(model));
            int affected = this.NonQueryExecuteSQL("[brb_CRUD_auction]", new Hashtable() { { "@table", model }, { "@type", CRUD.Create } });
            return affected > 0;
        }

#nullable enable
        public DataTable GetMenus(UserBase? _user)
        {
            var info = new Hashtable() { { "@user_id", -1 } };
            if (_user != null) info["@user_id"] = _user.UserTypeId;

            return this.GetSQLData("[brb_get_menus]", info);
        }
#nullable disable

        private DataTable GetDataModel(string _model_name)
        {
            string sql = $"DECLARE @table {_model_name}; SELECT * FROM @table;";
            return this.GetSQLData(sql);
        }
    }
}