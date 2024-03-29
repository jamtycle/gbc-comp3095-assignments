using System.Collections;
using System.Security.Cryptography;
using assignment1.Data;
using assignment1.Models.Auth;
using assignment1.Models.Generics;
using static System.Text.Encoding;

namespace assignment1.Libs
{
    public class Auth
    {
        private readonly UserBase user;
        private readonly UserBase dbuser;
        private readonly Hashtable errors = new();

        public Auth(string _session)
        {
            var info = new DBConnector().RecoverSession(_session);
            if (info is LoginModel @login) this.user = @login;
            else if (info is string @error) errors.Add("login-error", @error);
            else throw new Exception("🤨📸🤨📸🤨📸🤨📸🤨📸🤨📸🤨📸🤨📸🤨📸 Huh? 🤨📸🤨📸🤨📸🤨📸🤨📸🤨📸🤨📸🤨📸🤨📸");
        }

        public Auth(UserBase _user)
        {
            this.user = _user;
            if (this.user is LoginModel @user)
            {
                this.EncryptPassword();
                if (@user.Id == -1) // TODO: Check  
                {
                    var info = new DBConnector().LoginUser(@user);
                    if (info is LoginModel @dbuser) this.dbuser = @dbuser;
                    else errors.Add("login-error", (string)info);
                }
            }
            else if (this.user is RegistrationModel @reg)
            {
                @reg.VerifyPassword = null;
                EncryptPassword();
                this.GenerateEmailValidationKey();
            }
        }

        #region UserValidation
        private void GenerateEmailValidationKey()
        {
            string key = GenerateBase64(UTF8.GetBytes(Guid.NewGuid().ToString()));
            this.user.ValidationKey = key;
        }
        #endregion

        #region Sessions
        public void GenerateSession()
        {
            using SHA256 sha = SHA256.Create();
            byte[] edata = sha.ComputeHash(UTF8.GetBytes(new Random().Next(0, 1122091706).ToString()));

            user.SessionCookie = System.Convert.ToHexString(edata);
        }

        public bool ValidateSession()
        {
            if (errors.ContainsKey("login-error")) return false;

            // TODO: Get Session from DB.
            string dbsession = this.dbuser.SessionCookie;
            return user.SessionCookie.Equals(dbsession, StringComparison.Ordinal);
        }

        public bool HasActiveSession()
        {
            string dbsession = user.SessionCookie;
            return Utilities.ValidString(dbsession);
        }
        #endregion

        #region Two Factor Authentication
        public bool TwoFactorAuth(HttpResponse _response)
        {
            if (!this.dbuser.TwoFactorAuth) return false;

            int tf_code = new Random().Next(100000, 999999);
            string tf_code_encrypt = GenerateSHA256(tf_code);
            this.user.TwoFactorCode = tf_code_encrypt;

            if (!new DBConnector().SetTwoFactorCode(this.dbuser, tf_code_encrypt)) return false;

            WriteTFACookie(_response);
            string body = Persistent.TwoFactorEmail;
            body = string.Format(body, tf_code.ToString().ToCharArray().Select(x => x.ToString()).ToArray());
            string status = new MailTOGO.Sending(body, Persistent.EmailInfo)
            {
                IsHTML = true,
                Subject = "Best Auction - TFA",
                To = new string[1] { this.dbuser.Email },
            }.MailTOGO();

            return status.Equals("send");
        }

        private void WriteTFACookie(HttpResponse _response)
        {
            string cookie = $"{GenerateBase64(UTF8.GetBytes($"{this.dbuser.Id:X10}"))}_{this.user.GetType().GetProperty("RememberMe", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).GetValue(this.user, null)}";
            _response.Cookies.Append("user_tfa", cookie, new CookieOptions()
            {
                Expires = DateTime.Now.AddMinutes(10),
                Path = "/"
            });
        }

        public bool ValidateTwoFactor(int _code)
        {
            if (!this.user.TwoFactorAuth) return false;

            string dbtfcode = new DBConnector().GetTwoFactorCode(this.user);
            string tfcode = GenerateSHA256(_code);
            return dbtfcode.Equals(tfcode, StringComparison.Ordinal);
        }
        #endregion

        #region Passwords
        public void EncryptPassword()
        {
            //if (user.Password.Length < 8) throw new FormatException("Password MUST be more than 8 characters");
            string pass = user.Password;
            PeruvianSalt(ref pass);
            user.Password = pass;
            user.Password = GenerateSHA256(user.Password);
        }

        public bool ValidatePassword() // this MAY be changed later
        {
            if (errors.ContainsKey("login-error")) return false;

            string hashed_ps = this.dbuser.Password;
            //string pass = user.Password;
            //PeruvianSalt(ref pass);
            //user.Password = pass;
            return user.Password.Equals(hashed_ps, StringComparison.Ordinal);
        }

        public string GeneratePasswordResetCode()
        {
            if (this.user == null) throw new Exception("Non existing user passed to password reset.");

            string reset_code = Guid.NewGuid().ToString("X");
            new DBConnector().SetPasswordResetCode(this.user, reset_code);
            return reset_code;
        }
        #endregion

        #region Encryption
        private static string GenerateBase64(byte[] _word_bytes)
        {
            return System.Convert.ToBase64String(_word_bytes);
        }

        private static string GenerateSHA256(object _word) // maybe I just should use string as parameter xd
        {
            if (_word == null) return string.Empty;
            if (_word is string @word)
                if (!Utilities.ValidString(@word)) return @word;

            using SHA256 sha = SHA256.Create();
            return System.Convert.ToHexString(sha.ComputeHash(UTF8.GetBytes(_word.ToString())));
        }

        private static void PeruvianSalt(ref string _potatoes)
        {
            if (_potatoes == null) return;
            var char_nums = _potatoes.Select(x => x - '0');
            var salt = char_nums.Select((x, i) => Math.Tan(x));
            var pepper = char_nums.Select((x, i) => Math.Log(x, 6) / Math.Cos(x * Math.PI) - 0.2);
            var seasoner = salt.Zip(pepper, (x, y) => (x, y));
            var crackers = seasoner.Select(x => Math.Abs(x.x) + Math.Abs(x.y));

            int blade = 0;
            foreach (double blender in crackers)
            {
                //if (!double.IsFinite(blender)) blender = double.Epsilon;
                _potatoes = _potatoes.Insert(++blade, new string((char)(long)(!double.IsFinite(blender) ? double.Epsilon : blender), 1));
                blade++;
            }
        }
        #endregion

        #region Properties
        public Hashtable Errors { get => errors; }

        public UserBase User => user;
        #endregion
    }
}