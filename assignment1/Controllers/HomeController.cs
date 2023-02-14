using System.Diagnostics;
using assignment1.Libs;
using assignment1.Models;
using Microsoft.AspNetCore.Mvc;

namespace assignment1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;

    public HomeController(ILogger<HomeController> _logger)
    {
        this.logger = _logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // TODO: Requiere IndexModel.
        if (Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return RecoverUserSession();
        return View();
    }

    private IActionResult RecoverUserSession()
    {
        string session = Request.Cookies[Persistent.UserSession_Cookie] ?? string.Empty;
        if (!Utilities.ValidateString(session)) return View();

        Auth auth = new(session); // authorize the user with the session

        //TODO: get the menus from the database
        //TODO: send to the view a LayoutModel<T> with T as IndexModel

        return View(auth.User);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
