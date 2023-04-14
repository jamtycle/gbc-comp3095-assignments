using assignment1.Data;
using assignment1.Libs;
using assignment1.Models;
using assignment1.Models.Generics;
using assignment1.Models.Users;
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
            other.Reviews = new DBConnector().GetReviewByUser(_uid.Value);

            if (user == null) // Not logged in
                if (other == null) return RedirectToAction("Index", "Home");
                else return View("UserView", new LayoutModel<UserBase>()
                {
                    User = user,
                    Menus = this.GetMenus(user),
                    Data = other
                });

            // Logged in and same user
            if (user.Id.Equals(other.Id)) return View("UserProfile", new LayoutModel<UserBase>()
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

        [HttpGet("Seller")]
        public IActionResult SellersPage([FromQuery(Name = "uid")] int? _uid)
        {
            UserBase user = this.RecoverUserSession();

            if (!_uid.HasValue)
                if (user != null) _uid = user.Id;
                else return RedirectToAction("Index", "Home");

            UserBase other = new DBConnector().GetUser(_uid.Value);

            if (user == null) // Not logged in
                if (other == null) return RedirectToAction("Index", "Home");
                else return View("SellersPage", new LayoutModel<UserBase>()
                {
                    User = user,
                    Menus = this.GetMenus(user),
                    Data = other
                });

            if (user.Id.Equals(other.Id)) return View("SellersPage", new LayoutModel<UserBase>()
            {
                User = user,
                Menus = this.GetMenus(user),
                Data = user
            });

            return View("SellersPage", new LayoutModel<UserBase>()
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

            return View("UserProfile", new LayoutModel<UserBase>()
            {
                User = user,
                Menus = this.GetMenus(user),
                Data = user
            });
        }

        [HttpGet("EditProfile")]
        public IActionResult EditProfile()
        {
            if (!Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return UserPage(null);

            UserBase user = this.RecoverUserSession();
            return View("EditProfile", new LayoutModel<ProfileUser>()
            {
                User = user,
                Menus = this.GetMenus(user),
                Data = new ProfileUser(user)
            });
        }

        [HttpPost("EditProfile")]
        public IActionResult EditProfile(LayoutModel<ProfileUser> _model)
        {
            if (!Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return UserPage(null);

            if (!ModelState.IsValid) View(_model);
            UserBase user = this.RecoverUserSession();
            user.ProfilePic = GetImageFromRequest("Data.ProfilePic");
            if (new DBConnector().UpdateUser(user, true)) return UserPage(user.Id);

            return View(new LayoutModel<ProfileUser>()
            {
                User = user,
                Menus = this.GetMenus(user),
                Data = new ProfileUser(user)
            });
        }

        public byte[] GetImageFromRequest(string _name)
        {
            using var ms = new MemoryStream();
            IFormFile file = Request.Form.Files[_name];
            if (file == null) return Array.Empty<byte>();
            // Request.Form.Files[_name]
            file.CopyTo(ms);
            return ms.ToArray();
        }
    }
}