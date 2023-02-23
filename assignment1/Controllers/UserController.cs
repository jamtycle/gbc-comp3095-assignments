using assignment1.Data;
using assignment1.Models.Generics;
using Microsoft.AspNetCore.Mvc;

namespace assignment1.Controllers
{
    [Route("User")]
    public class UserController : BaseController
    {
        public UserController(ILogger<BaseController> _logger) : base(_logger)
        {
        }

        [HttpGet("Profile")]
        public IActionResult UserPage([FromQuery(Name = "uid")] int? _uid)
        {
            if (!_uid.HasValue) return RedirectToAction("Index", "Home");

            UserBase user = this.RecoverUserSession();
            Models.LayoutModel<UserBase> model = new()
            {
                User = user,
                Menu = this.GetMenus(user),
            };

            // Modify Version
            if (user == null)
            {
                UserBase otherUser = new DBConnector().GetUser(_uid.Value);
                if (otherUser == null) return RedirectToAction("Index", "Home");
                return View("UserView", otherUser);
            }

            model.User = user;
            if (user.Id.Equals(_uid.Value))
                return View("UserOwner", user);

            // Read-only Version
            return View("UserView", user);
        }

        [HttpPost("MakeUserSeller")]
        public IActionResult MakeSellerUser([FromQuery(Name = "uid")] int? _uid)
        {
            if(!_uid.HasValue) return View();

            UserBase user = this.RecoverUserSession();
            user.UserTypeId = Libs.Persistent.user_type_table.FirstOrDefault(x => x.UserTypeName.Equals("Sellers")).UserTypeId;

            if (new DBConnector().UpdateUser(user))
                return UserPage(_uid);

            return View(_uid);
        }
    }
}