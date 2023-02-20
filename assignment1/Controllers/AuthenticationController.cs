using System.Diagnostics;
using assignment1.Data;
using assignment1.Libs;
using assignment1.Models;
using assignment1.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace assignment1.Controllers
{
    public class AuthenticationController : Controller
    {
        #region Fields
        private readonly ILogger<AuthenticationController> logger;
        #endregion

        #region HTTPS - GET
        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return RedirectToAction("Index", "Home");
            return View();
            // Possibly session available
            // string session = Request.Cookies[Persistent.UserSession_Cookie] ?? string.Empty;
            // if (string.IsNullOrEmpty(session) || string.IsNullOrWhiteSpace(session)) return View();

            // var info = new DBConnector().RecoverSession(session);

            // if (info is LoginModel @login) return RedirectToAction("Index", "Home", @login);
            // else if (info is string @message) return View("AuthResult", new AuthInfoModel() { Title = "Session recovered!", Message = @message });
            // else return View();
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
        #endregion

        #region HTTPS - POST
        [HttpPost("Login")]
        public IActionResult Login(LoginModel _login)
        {
            if (!ModelState.IsValid) return View(_login);

            if (_login == null) return View(); // Shouldn't be needed, since LoginModel is not nulleable.

            Auth auth = new(_login);
            auth.GenerateSession();
            Response.Cookies.Append("user_session", _login.SessionCookie, new CookieOptions() { Expires = DateTime.Now.AddDays(7), Path = "/" });
            // Request.Cookies.Append(new KeyValuePair<string, string>("", ""));
            // new Auth(_login).ValidatePassword()

            // TODO: If the session cookie string is empty.
            if (auth.HasActiveSession()) return auth.ValidateSession() ? GoToIndex() : View("SessionError");

            if (auth.ValidatePassword())
            {
                auth.GenerateSession();
                // _login.MachineName = Environment.MachineName; // TODO: maybe this is not the user machine.
                return GoToIndex();
            }

            // if(auth.ValidatePassword())
            // TODO: Send the machine name & the cookie session to the database

            return View("AuthInfo");
        }

        [HttpPost("Register")]
        public IActionResult Register(RegistrationModel _user)
        {
            if (!ModelState.IsValid) return View(_user);

            Auth auth = new(_user);

            if (new DBConnector().NewUser((RegistrationModel)auth.User))
            {
                return GoToIndex();
                // return View("AuthResult", new AuthInfoModel() { Title = "Registration Success", Message = "The registration was a success, please log in now!" });
            }
            else return View(auth.User);
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