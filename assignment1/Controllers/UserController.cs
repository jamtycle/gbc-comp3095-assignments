using System;
using assignment1.Data;
using assignment1.Models;
using assignment1.Models.Generics;
using Microsoft.AspNetCore.Mvc;

namespace assignment1.Controllers
{
    [Route("User")]
    public class UserController : BaseController
    {
        public UserController(ILogger<BaseController> _logger) : base(_logger) { }

        [HttpGet("Profile")]
        public IActionResult UserPage([FromQuery(Name = "uid")] int? _uid)
        {
            UserBase user = this.RecoverUserSession();

            if (!_uid.HasValue)
                if (user != null) _uid = user.Id;
                else return RedirectToAction("Index", "Home");

            UserBase other = new DBConnector().GetUser(_uid.Value);

            if (user == null) // Not logged in
                if (other == null) return RedirectToAction("Index", "Home");
                else return View("UserView", new LayoutModel<UserBase>()
                {
                    User = user,
                    Menus = this.GetMenus(user),
                    Data = other
                });

            // Logged in and same user
            if (user.Id.Equals(other.Id)) return View("UserOwner", new LayoutModel<UserBase>()
            {
                User = user,
                Menus = this.GetMenus(user),
                Data = user
            });

            // Logged in and not same usr
            return View("UserView", new LayoutModel<UserBase>()
            {
                User = user,
                Menus = this.GetMenus(user),
                Data = other
            });
        }

        [HttpPost("MakeUserSeller")]
        public IActionResult MakeSellerUser(int? uid)
        {
            if (!uid.HasValue) return UserPage(uid);

            UserBase user = this.RecoverUserSession();
            if (user == null) return RedirectToAction("Index", "Home");
            if (!user.Id.Equals(uid)) return RedirectToAction("Index", "Home");

            user.UserTypeId = Libs.Persistent.user_type_table.FirstOrDefault(x => x.UserTypeName.Equals("Seller")).UserTypeId;
            if (!new DBConnector().UpdateUser(user, true))
            {
                ViewBag.MakeUserSeller_Error = "There was an error when trying to update the user 😔";
            }

            return View("UserOwner", new LayoutModel<UserBase>()
            {
                User = user,
                Menus = this.GetMenus(user),
                Data = user
            });
        }
    }
}