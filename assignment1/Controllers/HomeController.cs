using assignment1.Data;
using assignment1.Libs;
using assignment1.Models;
using assignment1.Models.Generics;
using assignment1.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace assignment1.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ILogger<HomeController> _logger) : base(_logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return GetLoggedMain;

            UserBase user = this.RecoverUserSession();
            DBConnector db = new();
            LayoutModel<IndexModel> model = new()
            {
                User = user,
                Menus = this.GetMenus(null),
                Data = new IndexModel()
                {
                    AuctionList = db.GetLastAuctions(DBConnector.LandingPageAuctionsOptions.Last50),
                    Carousel = db.GetLastAuctions(DBConnector.LandingPageAuctionsOptions.Carrousel)
                }
            };
            return await Task.Run(() => View(model));
        }

        [HttpGet("Search")]
        public IActionResult Search([FromQuery(Name = "search")] string _search)
        {
            UserBase user = this.RecoverUserSession();
            LayoutModel<SearchModel> model = new()
            {
                User = user,
                Menus = this.GetMenus(user),
                Data = new SearchModel()
                {
                    SearchText = _search ?? string.Empty,
                    Search = new DBConnector().SearchAuctions(_search)
                }
            };

            return View(model);
        }

        private IActionResult GetLoggedMain
        {
            get
            {
                DBConnector db = new();
                IndexModel index = new()
                {
                    Carousel = db.GetLastAuctions(DBConnector.LandingPageAuctionsOptions.Carrousel),
                    AuctionList = db.GetLastAuctions(DBConnector.LandingPageAuctionsOptions.Last50)
                };

                UserBase user = this.RecoverUserSession();
                if (user == null) this.RemoveUserSession();

                LayoutModel<IndexModel> model = new()
                {
                    User = user,
                    Menus = this.GetMenus(user),
                    Data = index
                };

                return View(model);
            }
        }
    }
}