using System.Diagnostics;
using assignment1.Data;
using assignment1.Libs;
using assignment1.Models;
using assignment1.Models.Auth;
using assignment1.Models.Generics;
using Microsoft.AspNetCore.Mvc;

namespace assignment1.Controllers
{
    [Route("Auth")]
    public class AuthenticationController : Controller
    {
        #region Fields
        private readonly ILogger<AuthenticationController> logger;
        #endregion

        #region HTTPS - GET
        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return GoToIndex();
            return View();
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            if (Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return GoToIndex();
            return View();
        }

        [HttpGet("AuthResult")]
        public IActionResult AuthResult(AuthInfoModel _model)
        {
            return View(_model);
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            if (!Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return GoToIndex();

            Response.Cookies.Delete(Persistent.UserSession_Cookie);
            return GoToIndex();
        }

        [HttpGet("ValidateUser")]
        public IActionResult ValidateUser(string key)
        {
            if (new DBConnector().UserValidationKey(key))
            {
                ViewBag.Message = "Yout account have been validated 👍";
                return View();
            }

            // Guid.NewGuid().ToByteArray()
            // System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String("AJHZIuGnlEK0t0+z/Y9wWg=="))

            ViewBag.Message = "There was a problem while validating your account 😔";
            return View();
        }

        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet("ResetPassword")]
        public IActionResult ResetPassword([FromQuery(Name = "key")] string _key)
        {
            string pk = _key.Split('-').FirstOrDefault();
            byte[] b = Convert.FromHexString(_key.Split('-').LastOrDefault());
            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);
            int uid = BitConverter.ToInt32(b, 0);
            UserBase user = new DBConnector().GetUser(uid);
            if (new DBConnector().ValidatePasswordResetCode(user, pk)) return View(new ResetPasswordModel() { Key = _key });
            else return GoToIndex();
        }
        #endregion

        #region HTTPS - POST
        [HttpPost("Login")]
        public IActionResult Login(LoginModel _login)
        {
            if (Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return GoToIndex();
            if (!ModelState.IsValid) return View(_login);

            Auth auth = new(_login);

            if (auth.HasActiveSession()) return auth.ValidateSession() ? GoToIndex() : View(auth.User);

            if (auth.ValidatePassword())
            {
                auth.GenerateSession();
                if (new DBConnector().SetSession(auth.User.Username, auth.User.SessionCookie))
                {
                    Response.Cookies.Append("user_session", _login.SessionCookie, new CookieOptions()
                    {
                        Expires = DateTime.Now.AddDays(_login.RememberMe ? 7 : 1),
                        Path = "/"
                    });
                    return GoToIndex();
                }
                return View(auth.User);
            }

            return View(auth.User);
        }

        [HttpPost("Register")]
        public IActionResult Register(RegistrationModel _user)
        {
            if (Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return GoToIndex();
            if (!ModelState.IsValid) return View(_user);

            Auth auth = new(_user);

            if (new DBConnector().NewUser((RegistrationModel)auth.User))
            {
                string body = string.Format(Persistent.VerificationEmail, _user.Username, $"http://{Request.Host}/Auth/ValidateUser?key={auth.User.ValidationKey}");
                string status = new MailTOGO.Sending(body, Persistent.EmailInfo)
                {
                    IsHTML = true,
                    Subject = "Best Auction - Email Verification",
                    To = new string[1] { _user.Email },
                }.MailTOGO();

                if (status.Equals("send")) return GoToIndex();
                else return View(auth.User);
                // return View("AuthResult", new AuthInfoModel() { Title = "Registration Success", Message = "The registration was a success, please log in now!" });
            }
            else return View(auth.User);
        }

        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(string email)
        {
            UserBase user = new DBConnector().GetUser(email);
            if (user == null) return View();

            string code = Guid.NewGuid().ToString("N");
            if (!new DBConnector().SetPasswordResetCode(user, code)) return View();

            string status = new MailTOGO.Sending(Persistent.EmailInfo)
            {
                To = new string[] { email },
                IsHTML = true,
                Message = string.Format(Persistent.ResetPassword, user.Username, $"http://{Request.Host}/Auth/ResetPassword?key={code}-{user.Id:X10}"),
                Subject = "Password Reset"
            }.MailTOGO();

            if (!status.Equals("send")) return GoToIndex();

            return View();
        }

        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordModel _model)
        {
            if (!this.ModelState.IsValid)
            {
                if (this.ModelState["Key"].Errors.Any()) return GoToIndex();
                return View(_model);
            }

            string pk = _model.Key.Split('-').FirstOrDefault();
            byte[] b = Convert.FromHexString(_model.Key.Split('-').LastOrDefault());
            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);
            int uid = BitConverter.ToInt32(b, 0);
            UserBase user = new DBConnector().GetUser(uid);
            user.Password = _model.Password;
            Auth auth = new (user);
            if (new DBConnector().ConsumePasswordResetCode(auth.User, pk)) return RedirectToAction("Login", "Auth");
            else return View();
        }
        #endregion

        #region Helpers
        private IActionResult GoToIndex() => RedirectToAction("Index", "Home");
        #endregion

        #region Logging & Error Handling
        public AuthenticationController(ILogger<AuthenticationController> _logger)
        {
            this.logger = _logger;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}