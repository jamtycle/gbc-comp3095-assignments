using assignment1.Data;

namespace assignment1.Libs
{
    public static class Persistent
    {
        public const string UserSession_Cookie = "user_session";
        private static readonly string cnxstring = "RGF0YSBTb3VyY2U9bWlpc250YW5jaWEuYzFuajlvN21ncXN4LnVzLWVhc3QtMS5yZHMuYW1hem9uYXdzLmNvbSwxNDMzO0luaXRpYWwgQ2F0YWxvZz1EZWNvbXByZXNzb3I7UGVyc2lzdCBTZWN1cml0eSBJbmZvPVRydWU7VXNlciBJRD1kZWNvbXByZXNzb3I7UGFzc3dvcmQ9ZnRQc1lXaktwUHdVZlU1QjtFbmNyeXB0PUZhbHNl";
        public static string CnxString { get => cnxstring; }

        public static readonly IEnumerable<dynamic> user_type_table = new DBConnector().GetUserTypes();
    }
}