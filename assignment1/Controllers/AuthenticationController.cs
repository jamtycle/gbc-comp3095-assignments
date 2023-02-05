using System.Diagnostics;
using assignment1.Libs;
using Microsoft.AspNetCore.Mvc;

namespace assignment1.Models
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
            return View();
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }
        #endregion

        #region HTTPS - POST
        [HttpPost("Login")]
        public IActionResult Login(LoginModel _login)
        {
            if (_login == null) return View(); // Shouldn't be needed, since LoginModel is not nulleable.

            Auth auth = new(_login);
            // new Auth(_login).ValidatePassword()

            // TODO: If the session cookie string is empty.
            if (auth.HasActiveSession())
            {
                return auth.ValidateSession() ? View("Home") : View("SessionError");
            }

            if (auth.ValidatePassword())
            {
                _login.SessionCookie = auth.GenerateSession();
                _login.MachineName = Environment.MachineName; // TODO: maybe this is not the user machine.
                return View("Home");
            }

            // if(auth.ValidatePassword())
            // TODO: Send the machine name & the cookie session to the database

            return View("AuthInfo");
        }

        [HttpPost("Register")]  
        public IActionResult Register(RegistrationModel _user)
        {
            if (!ModelState.IsValid) return View(_user);

            return View("Home");
        }
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