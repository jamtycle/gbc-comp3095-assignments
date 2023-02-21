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

        public DataTable GetLastAuctions()
        {
            return this.GetSQLData("[brb_get_last_auctions]", new Hashtable());
        }

        public bool AddBid(BidModel _bid)
        {
            var model = GetDataModel("BidType");
            model.Rows.Add(_bid.ToDataRow(model));
            int affected = this.NonQueryExecuteSQL("[brb_CRUD_bid]", new Hashtable() { { "@table", model }, { "@type", CRUD.Create } });
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