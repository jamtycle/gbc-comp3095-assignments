using System.Diagnostics;
using assignment1.Data;
using assignment1.Libs;
using assignment1.Models;
using assignment1.Models.Auction;
using assignment1.Models.Generics;
using assignment1.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace assignment1.Controllers;

public class HomeController : BaseController
{
    public HomeController(ILogger<HomeController> _logger) : base(_logger)
    {
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return GetLoggedMain;

        LayoutModel model = new() { Menus = this.GetMenus(null) };
        return View(model);
    }

    [HttpGet("Search")]
    public IActionResult Search([FromQuery(Name = "search")] string _search)
    {
        UserBase user = this.RecoverUserSession();
        LayoutModel<IEnumerable<AuctionModel>> model = new()
        {
            Menus = this.GetMenus(user),
            Data = new DBConnector().SearchAuctions(_search)
        };
        // new DBConnector().SearchAuctions(_search);
        return View(model);
    }

    private IActionResult GetLoggedMain
    {
        get
        {
            UserBase user = this.RecoverUserSession();
            DBConnector db = new();
            IndexModel index = new()
            {
                Carousel = db.GetLastAuctions(DBConnector.LandingPageAuctionsOptions.Carrousel),
                AuctionList = db.GetLastAuctions(DBConnector.LandingPageAuctionsOptions.Last50)
            };

            LayoutModel<IndexModel> model = new()
            {
                Menus = this.GetMenus(user),
                Data = index
            };

            return View(model);
        }
    }
}
