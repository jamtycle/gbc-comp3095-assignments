using System.Collections;
using System.Data;
using assignment1.Libs;
using assignment1.Models;
using assignment1.Models.Auction;
using assignment1.Models.Auth;
using assignment1.Models.Generics;
using assignment1.Models.Home;
using assignment1.Models.Users;
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

        public byte[] GetAuctionPic(int _auction_id)
        {
            DataTable info = this.GetSQLData("[brb_get_auction_image]", new Hashtable { { "@auction_id", _auction_id } });
            return info.Rows.Count == 0 ? Array.Empty<byte>() : (byte[])(info.Rows[0]["image"] is DBNull ? Array.Empty<byte>() : info.Rows[0]["image"]);
        }

        public enum LandingPageAuctionsOptions : byte { Carrousel, Last50, Last100 }
        public IEnumerable<AuctionModel> GetLastAuctions(LandingPageAuctionsOptions _option)
        {
            DataTable table = this.GetSQLData("[brb_get_last_auctions]", new Hashtable() { { "@option", (byte)_option } });
            foreach (DataRow row in table.Rows)
                yield return new AuctionModel(row);
        }

        public IEnumerable<AuctionModel> SearchAuctions(SearchModel _search)
        {
            // string _search, string _condition, double _min_price, double _max_price, string _status

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
            DataTable data = this.GetSQLData("[brb_auction_search]", new Hashtable()
            {
                { "@search", _search.SearchText },
                { "@condition", _search.ConditionText },
                { "@min_price", _search.MinPrice },
                { "@max_price", _search.MaxPrice },
                { "@status", _search.Status },
            });

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

        public bool UpdateUser(UserBase _user, bool _deep_copy = false)
        {
            var model = GetDataModel("UserType");
            model.Rows.Add(_user.ToDataRow(model, _deep_copy));
            int affected = this.NonQueryExecuteSQL("[brb_CRUD_user]", new Hashtable() { { "@table", model }, { "@type", CRUD.Update } });
            return affected > 0;
        }

        public bool SetSession(string _username, string _session)
        {
            return this.NonQueryExecuteSQL("[brb_set_session]", new Hashtable() { { "@username", _username }, { "@session", _session } }) > 0;
        }

        public int NewAuction(AuctionModel _auction)
        {
            var model = this.GetDataModel("AuctionType");
            model.Rows.Add(_auction.ToDataRow(model));
            DataTable info = this.GetSQLData("[brb_CRUD_auction]", new Hashtable() { { "@table", model }, { "@type", CRUD.Create } });
            // int affected = this.NonQueryExecuteSQL("[brb_CRUD_auction]", new Hashtable() { { "@table", model }, { "@type", CRUD.Create } });
            if (info.Rows.Count == 0) return -1;
            return (int)info.Rows[0]["id"];
        }

        public bool UpdateAuction(AuctionModel _auction, bool _deep_copy = false)
        {
            var model = this.GetDataModel("AuctionType");
            model.Rows.Add(_auction.ToDataRow(model, _deep_copy));
            return this.NonQueryExecuteSQL("[brb_CRUD_auction]", new Hashtable() { { "@table", model }, { "@type", CRUD.Update } }) == 1;
        }

        public bool UserValidationKey(string _key)
        {
            return this.NonQueryExecuteSQL("[brb_user_validation_key]", new Hashtable() { { "@key", _key } }) > 0;
        }

        private DataTable GetDataModel(string _model_name)
        {
            string sql = $"DECLARE @table {_model_name}; SELECT * FROM @table;";
            return this.GetSQLData(sql);
        }

        public UserBase GetUser(int _id)
        {
            DataTable info = this.GetSQLData("[brb_get_user]", new Hashtable() { { "@user_id", _id } });
            if (info.Rows.Count == 0) return null;
            return new LoginModel(info.Rows[0]);
        }

        public UserBase GetUser(string _email)
        {
            DataTable info = this.GetSQLData("[brb_get_user_by_email]", new Hashtable() { { "@email", _email } });
            if (info.Rows.Count == 0) return null;
            return new LoginModel(info.Rows[0]);
        }

        public bool SetPasswordResetCode(UserBase _user, string _code)
        {
            return this.NonQueryExecuteSQL("[brb_set_password_reset]", new Hashtable() { { "@user_id", _user.Id }, { "@reset_code", _code } }) == 1;
        }

        public bool ValidatePasswordResetCode(UserBase _user, string _code)
        {
            return this.GetSQLData("[brb_validate_password_reset]", new Hashtable() { { "@user_id", _user.Id }, { "@reset_code", _code } }).Rows.Count == 1;
        }

        public bool ConsumePasswordResetCode(UserBase _user, string _code)
        {
            return this.NonQueryExecuteSQL("[brb_consume_password_reset]", new Hashtable() { { "@user_id", _user.Id }, { "@reset_code", _code }, { "@password", _user.Password } }) == 1;
        }

        public bool SetTwoFactorCode(UserBase _user, string _tf_code)
        {
            return this.NonQueryExecuteSQL("[brb_tf_set_code]", new Hashtable() { { "@user_id", _user.Id }, { "@tf_code", _tf_code } }) == 1;
        }

        public string GetTwoFactorCode(UserBase _user)
        {
            DataTable info = this.GetSQLData("[brb_tf_get_code]", new Hashtable() { { "@user_id", _user.Id } });
            if (info.Rows.Count == 0) return string.Empty;
            return (string)info.Rows[0]["two_factor_code"];
        }

        public byte[] GetUserPic(int _id)
        {
            DataTable info = this.GetSQLData("[brb_get_user_pic]", new Hashtable() { { "@user_id", _id } });
            return info.Rows.Count == 0 ? Array.Empty<byte>() : (byte[])(info.Rows[0]["profile_pic"] is DBNull ? Array.Empty<byte>() : info.Rows[0]["profile_pic"]);
        }

        public bool UpdateProfile(UserBase _user)
        {
            var model = GetDataModel("UserType");
            model.Rows.Add(_user.ToDataRow(model));
            return this.NonQueryExecuteSQL("[brb_update_profile]", new Hashtable() { { "@user", model } }) == 1;
        }

        public IEnumerable<UserBase> GetAllUsers()
        {
            DataTable info = this.GetSQLData("[brb_get_all_users]");
            foreach (DataRow row in info.Rows)
                yield return new ManagedUser(row);
        }
    }
}