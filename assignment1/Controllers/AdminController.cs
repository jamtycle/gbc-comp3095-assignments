using assignment1.Data;
using assignment1.Libs;
using assignment1.Models;
using assignment1.Models.Users;
using assignment1.Models.Generics;
using Microsoft.AspNetCore.Mvc;
using assignment1.Models.Auction;

namespace assignment1.Controllers
{
    [Route("Admin")]
    public class AdminController : BaseController
    {
        public AdminController(ILogger<AdminController> _logger) : base(_logger) { }

        [HttpGet("AdminPage")]
        public IActionResult AdminPage()
        {
            if (!Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return RedirectToAction("Index", "Home");

            UserBase user = this.RecoverUserSession();

            if (user == null) return Forbid();
            else if (user.UserTypeId != Persistent.user_type_table.FirstOrDefault(x => x.UserTypeName.Equals("Admin")).UserTypeId) return Forbid();

            return View(new LayoutModel<IEnumerable<UserBase>>()
            {
                User = user,
                Menus = this.GetMenus(user),
                Data = new DBConnector().GetAllUsers(),
            });
        }

        [HttpGet("AdminEditUser")]
        public IActionResult AdminEditUser([FromQuery(Name = "uid")] int? _uid)
        {
            if (!Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return RedirectToAction("Index", "Home");

            UserBase user = this.RecoverUserSession();

            if (user == null) return Forbid();

            else if (user.UserTypeId != Persistent.user_type_table.FirstOrDefault(x => x.UserTypeName.Equals("Admin")).UserTypeId) return Forbid();

            // if (!_uid.HasValue) return RedirectToAction("AdminPage", "Admin");

            UserBase other = new DBConnector().GetUser(_uid.Value);

            if (other == null) return RedirectToAction("AdminPage", "Admin");

            return View(new LayoutModel<ProfileUser>()
            {
                User = user,
                Menus = this.GetMenus(user),
                Data = new ProfileUser(other)
            });
        }

        [HttpPost("AdminEditUser")]
        public IActionResult AdminEditUser(int id, ProfileUser _user)
        {   
            
            if (!Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return RedirectToAction("Index", "Home");

            UserBase user = this.RecoverUserSession();

            if (user == null) return Forbid();

            else if (user.UserTypeId != Persistent.user_type_table.FirstOrDefault(x => x.UserTypeName.Equals("Admin")).UserTypeId) return Forbid();

            if (_user == null) return RedirectToAction("AdminPage", "Admin");

            UserBase other = new DBConnector().GetUser(id);

            if (other == null) return RedirectToAction("AdminPage", "Admin");

       
            other.Username = _user.Username;
            other.Email = _user.Email;
            other.FirstName = _user.FirstName;
            other.LastName = _user.LastName;

            new DBConnector().UpdateUser(other, true);

            return RedirectToAction("AdminPage", "Admin");
        }

        [HttpGet("AdminAuction")]
        public IActionResult AdminAuction([FromQuery(Name = "aid")] int? _aid)
        {
            if (!Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return RedirectToAction("Index", "Home");

            UserBase user = this.RecoverUserSession();

            if (user == null) return Forbid();

            else if (user.UserTypeId != Persistent.user_type_table.FirstOrDefault(x => x.UserTypeName.Equals("Admin")).UserTypeId) return Forbid();

            if (!_aid.HasValue) return RedirectToAction("AdminPage", "Admin");

            IEnumerable<AuctionModel> auction = new DBConnector().GetLastAuctions(DBConnector.LandingPageAuctionsOptions.Last50);

            if (auction == null) return RedirectToAction("AdminPage", "Admin");

            return View(new LayoutModel<IEnumerable<AuctionModel>>()
            {
                User = user,
                Menus = this.GetMenus(user),
                Data = auction
            });
        }

        [HttpPost("AdminEditAuction")]
        public IActionResult AdminEditAuction(int id, AuctionModel _auction)
        {
            if (!Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return RedirectToAction("Index", "Home");

            UserBase user = this.RecoverUserSession();

            if (user == null) return Forbid();

            else if (user.UserTypeId != Persistent.user_type_table.FirstOrDefault(x => x.UserTypeName.Equals("Admin")).UserTypeId) return Forbid();

            if (_auction == null) return RedirectToAction("AdminAuctions", "Admin");

            AuctionModel auction = new DBConnector().GetAuction(id);

            if (auction == null) return RedirectToAction("AdminAuctions", "Admin");

            auction.AuctionName = _auction.AuctionName;
            

            new DBConnector().UpdateAuction(auction, true);

            return RedirectToAction("AdminPage", "Admin");
        }
    }
}