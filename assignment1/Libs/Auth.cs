using System.Security.Cryptography;
using assignment1.Models.Generics;
using static System.Text.Encoding;

namespace assignment1.Libs
{
    public class Auth
    {
        private readonly UserBase user;

        public Auth(UserBase _user)
        {
            this.user = _user;
        }

        #region Sessions
        public string GenerateSession()
        {
            using SHA256 sha = SHA256.Create();
            byte[] edata = sha.ComputeHash(UTF8.GetBytes(new Random().Next(0, 1122091706).ToString()));

            return System.Convert.ToHexString(edata);
        }

        public bool ValidateSession()
        {
            // TODO: Get Session from DB.
            string session = string.Empty;
            return user.SessionCookie.Equals(session, StringComparison.Ordinal);
        }

        public bool HasActiveSession()
        {
            string db_session = string.Empty; // TODO: Get session from DB.
            return string.IsNullOrEmpty(db_session) || string.IsNullOrWhiteSpace(db_session);
        }
        #endregion

        #region Passwords
        public string EncryptPassword()
        {
            if (user.Password.Length < 8) throw new FormatException("Password MUST be more than 8 characters");
            string pass = user.Password;
            PeruvianSalt(ref pass);
            user.Password = pass;
            return GenerateSHA256(user.Password);
        }

        public bool ValidatePassword() // this MAY be changed later
        {
            string hashed_ps = string.Empty; // TODO: Get hashed password from DB.
            string pass = user.Password;
            PeruvianSalt(ref pass);
            user.Password = pass;
            return GenerateSHA256(user.Password).Equals(hashed_ps, StringComparison.Ordinal);
        }
        #endregion

        #region Encryption
        private static string GenerateSHA256(object _word) // maybe I just should use string as parameter xd
        {
            if (_word == null) return string.Empty;
            if (_word is not string) _word = _word.ToString() ?? string.Empty;
            if (_word.Equals(string.Empty)) return (string)_word;

            using SHA256 sha = SHA256.Create();
            return System.Convert.ToHexString(sha.ComputeHash(UTF8.GetBytes((string)_word)));
        }

        private static void PeruvianSalt(ref string _potatoes)
        {
            var char_nums = _potatoes.Select(x => x - '0');
            var salt = char_nums.Select((x, i) => Math.BitDecrement(x));
            var pepper = char_nums.Select((x, i) => Math.Log(x, 6) / Math.Cos(x * Math.PI) - 0.2);
            var seasoner = salt.Zip(pepper, (x, y) => (x, y));
            var crakers = seasoner.Select(x => Math.Abs(x.x) + Math.Abs(x.y));

            int blade = 1;
            foreach (double blender in crakers)
                _potatoes = _potatoes.Insert(blade, new string((char)System.Convert.ToInt32(blender), 1));
        }
        #endregion
    }
}