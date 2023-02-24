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

        #region Email
        public static string VerificationEmail
        {
            get {
                return @"<div style='display: flex; flex-direction: column; font-family: 'Segoe UI', sans-serif;'>
                            <div
                                style='display: flex; border-radius: 30px; flex-direction: column; flex-wrap: nowrap; align-items: center; padding: 1em; color: #000'>
                                <h2>Hi {0}!</h2>
                                <p style='text-align: center;'>Thanks for register to our webpage! <br> Please click in the
                                    link below to verify your account!</p>

                                <a href='{1}' target='_blank'
                                    style='text-decoration: none; background-color: #3B71CA; color: #fff; padding: .5em .9em; border-radius: 20px; font-weight: bold;'>Verify
                                    your email!</a>
                            </div>
                         </div>";
            }
        }

        public static Hashtable EmailInfo
        {
            get
            {
                return new()
                {
                    { "account", "Decompresor.Agrosoft@gmail.com" },
                    { "pass",    "$:Agrosoft$:Who++" }
                };
            }
        }
        #endregion
    }
}