using assignment1.Data;
using assignment1.Models.Generics;
using Microsoft.AspNetCore.Mvc;
using System;

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
                Menus = this.GetMenus(user),
            };

            // Read-only Version
            if (user == null)
            {
                UserBase otherUser = new DBConnector().GetUser(_uid.Value);
                if (otherUser == null) return RedirectToAction("Index", "Home");
                model.Data = otherUser;
                return View("UserView", model);
            }

            // Modify Version
            if (user.Id.Equals(_uid.Value))
            {
                model.Data = user;
                return View("UserOwner", model);
            }

            model.Data = new DBConnector().GetUser(_uid.Value);
            if (user == null) return RedirectToAction("Index", "Home");
            return View("UserView", model);
        }

        [HttpPost("MakeUserSeller")]
        public IActionResult MakeSellerUser(int? uid)
        {
            if(!uid.HasValue) return View("UserView", uid);
            
            UserBase user = this.RecoverUserSession();
            user.UserTypeId = Libs.Persistent.user_type_table.FirstOrDefault(x => x.UserTypeName.Equals("Sellers")).UserTypeId;
            new DBConnector().UpdateUser(user);
        
            Models.LayoutModel<UserBase> model = new()
            {
                User = user,
                Menus = this.GetMenus(user),
            };
            if (user == null)
            {
                UserBase otherUser = new DBConnector().GetUser(uid.Value);
                if (otherUser == null) return RedirectToAction("Index", "Home");
                model.Data = otherUser;
                return View("UserOwner", model);
            }
            if (user.Id.Equals(uid.Value))
            {
                model.Data = user;
                return View("UserOwner", model);
            }

            user.UserTypeId = Libs.Persistent.user_type_table.FirstOrDefault(x => x.UserTypeName.Equals("Sellers")).UserTypeId;

            if (new DBConnector().UpdateUser(user))
                return UserPage(uid);

            return View(uid);
        }
    }
}