using System.Diagnostics;
using assignment1.Data;
using assignment1.Libs;
using assignment1.Models;
using assignment1.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace assignment1.Controllers
{
    public class ReviewController : Controller
    {
        [HttpPost("AuctionReview")]
        public IActionResult Auction(ReviewModel _review)
        {
            if (Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid) return View(_review);

            return View();
        }
        [HttpPost("SellerReview")]
        public IActionResult SellersPage(ReviewModel _review)
        {
            if (Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid) return View(_review);

            return View();
        }
    }
}
