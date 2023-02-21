using System.Diagnostics;
using assignment1.Data;
using assignment1.Libs;
using assignment1.Models;
using assignment1.Models.Generics;
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

    private IActionResult GetLoggedMain
    {
        get
        {
            UserBase user = this.RecoverUserSession();

            LayoutModel<UserBase> model = new()
            {
                Menus = this.GetMenus(user),
                Data = user
            };

            return View(model);
        }
    }

    // private IActionResult RecoverUserSession()
    // {
    //     string session = Request.Cookies[Persistent.UserSession_Cookie] ?? string.Empty;
    //     if (!Utilities.ValidateString(session)) return View();

    //     Auth auth = new(session); // authorize the user with the session

    //     // TODO: get the menus from the database
    //     // TODO: send to the view a LayoutModel<T> with T as IndexModel
    //     LayoutModel<Models.Generics.UserBase> model = new()
    //     {
    //         Menus = new DBConnector().GetMenus(auth.User.UserTypeId),
    //         Data = auth.User
    //     };

    //     return View(model);
    // }
}
