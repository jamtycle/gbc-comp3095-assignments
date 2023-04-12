using System.Collections;
using assignment1.Data;

namespace assignment1.Libs
{
    public static class Persistent
    {
        public const string UserSession_Cookie = "user_session";
        private static readonly string cnxstring = "RGF0YSBTb3VyY2U9bWlpc250YW5jaWEuYzFuajlvN21ncXN4LnVzLWVhc3QtMS5yZHMuYW1hem9uYXdzLmNvbSwxNDMzO0luaXRpYWwgQ2F0YWxvZz1EZWNvbXByZXNzb3I7UGVyc2lzdCBTZWN1cml0eSBJbmZvPVRydWU7VXNlciBJRD1kZWNvbXByZXNzb3I7UGFzc3dvcmQ9ZnRQc1lXaktwUHdVZlU1QjtFbmNyeXB0PUZhbHNl";
        public static string CnxString { get => cnxstring; }

        public static readonly IEnumerable<dynamic> user_type_table = new DBConnector().GetUserTypes();

        public static dynamic InfoModel(string _title, string _message, string _button_text, string _action, string _page_title = "Information", string _controller = "") => new
        {
            PageTitle = _page_title,
            Title = _title,
            Message = _message,
            ButtonText = _button_text,
            Controller = _controller,
            Action = _action
        };

        #region Email
        public static string VerificationEmail
        {
            get
            {
                return "<div\n    style=\"display: grid; border-radius: 30px; flex-direction: column; flex-wrap: nowrap; align-items: center; padding: 1em; color: #000\">\n\n    <h2 style=\"text-align: center;\">Hi {0}!</h2>\n    <p style=\"text-align: center;\">Thanks for register to our webpage! <br> Please click\n        in the\n        link below to verify your account!</p>\n\n    <a href=\"{1}\" target=\"_blank\"\n        style=\"text-decoration: none; background-color: #3B71CA; color: #fff; padding: .5em .9em; border-radius: 20px; font-weight: bold; width: fit-content; margin: auto;\">Verify\n\n        your email!</a>\n</div>";
            }
        }

        public static string ResetPassword
        {
            get
            {
                return "<div style=\"display: grid; border-radius: 30px; flex-direction: column; flex-wrap: nowrap; align-items: center; padding: 1em; color: #000\"><h2 style=\"text-align: center;\">Houston we have a problem!</h2> <p style=\"text-align: center;\"> Hey {0} it appears that you lost your password, but don't worry, we won't tell 🤐! <br>Please click the button below to reset your password. </p><a href=\"{1}\" target=\"_blank\" style=\"text-decoration: none; background-color: #3B71CA; color: #fff; padding: .5em .9em; border-radius: 20px; font-weight: bold; width: fit-content; margin: auto;\">Reset your password</a></div>";
            }
        }

        public static string TwoFactorEmail
        {
            get
            {
                return "<div style=\"display: grid; border-radius: 30px; flex-direction: column; flex-wrap: nowrap; align-items: center; padding: 1em; color: #000\">  <h2 style=\"text-align: center;\">Two Factor Authentication</h2>  <div style=\"display: flex; margin: auto;\">      <h3 style=\"border: 1px solid gray; border-radius: 5px; padding: .6em .5em; font-size: 2.4em; margin: .5em .2em\">{0}</h3>      <h3 style=\"border: 1px solid gray; border-radius: 5px; padding: .6em .5em; font-size: 2.4em; margin: .5em .2em\">{1}</h3>      <h3 style=\"border: 1px solid gray; border-radius: 5px; padding: .6em .5em; font-size: 2.4em; margin: .5em .2em\">{2}</h3>      <h3 style=\"border: 1px solid gray; border-radius: 5px; padding: .6em .5em; font-size: 2.4em; margin: .5em .2em\">{3}</h3>      <h3 style=\"border: 1px solid gray; border-radius: 5px; padding: .6em .5em; font-size: 2.4em; margin: .5em .2em\">{4}</h3>      <h3 style=\"border: 1px solid gray; border-radius: 5px; padding: .6em .5em; font-size: 2.4em; margin: .5em .2em\">{5}</h3>  </div>  <p style=\"text-align: center;\"> Don't share this code with anyone! 🤫 <br> Now go and paste this code to log in.</p></div>";
            }
        }

        public static Hashtable EmailInfo
        {
            get
            {
                return new()
                {
                    { "account", "no.reply.jam.helper@gmail.com" },
                    { "pass",    "mqaobscyjiybxixr" }
                };
            }
        }
        #endregion
    }
}