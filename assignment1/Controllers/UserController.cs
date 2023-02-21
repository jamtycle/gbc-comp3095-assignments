using assignment1.Data;
using assignment1.Models.Generics;
using Microsoft.AspNetCore.Mvc;

namespace assignment1.Controllers
{
    public class UserController : BaseController
    {
        public UserController(ILogger<BaseController> _logger) : base(_logger)
        {
        }

        [HttpGet("User")]
        public IActionResult UserPage([FromQuery(Name = "uid")] int? _uid)
        {
            if (!_uid.HasValue) return View("UserFallback");

            UserBase user = this.RecoverUserSession();

            // Modify Version
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