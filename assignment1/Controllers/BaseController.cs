using System.Diagnostics;
using assignment1.Data;
using assignment1.Libs;
using assignment1.Models;
using assignment1.Models.Generics;
using Microsoft.AspNetCore.Mvc;

namespace assignment1.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly ILogger<BaseController> logger;

        public BaseController(ILogger<BaseController> _logger)
        {
            this.logger = _logger;
        }

        protected virtual UserBase RecoverUserSession()
        {
            string session = Request.Cookies[Persistent.UserSession_Cookie] ?? string.Empty;
            if (!Utilities.ValidString(session)) return null;
            return new Auth(session).User;
        }

        protected virtual void RemoveUserSession()
        {
            string session = Request.Cookies[Persistent.UserSession_Cookie] ?? string.Empty;
            if (!Utilities.ValidString(session)) return;
            Response.Cookies.Append(Persistent.UserSession_Cookie, "", new CookieOptions() { Expires = DateTime.Now.AddDays(-1), Path = "/" });
        }

        protected virtual IEnumerable<MenuModel> GetMenus(UserBase _user)
        {
            int user_type_id = -1;
            if (_user != null) user_type_id = _user.UserTypeId;
            return new DBConnector().GetMenus(user_type_id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}