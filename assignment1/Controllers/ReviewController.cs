using assignment1.Data;
using assignment1.Libs;
using assignment1.Models.Reviews;
using Microsoft.AspNetCore.Mvc;

namespace assignment1.Controllers
{
    [Route("Review")]
    public class ReviewController : Controller
    {
        [HttpPost("AddReview")]
        public IActionResult AddReview(ReviewModel _review)
        {
            if (!Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = string.Join('\n', ModelState.Select(x => string.Join('\n', x.Value.Errors.Select(e => e.ErrorMessage))));
                return RedirectToAction("AuctionPage", "Auction", new { aid = _review.AuctionId });
            }

            if (!new DBConnector().AddReview(_review))
            {
                TempData["Error"] = "Something went wrong 🥲";
                return RedirectToAction("AuctionPage", "Auction", new { aid = _review.AuctionId });
            }

            TempData["Rating"] = "Thanks for your review 😄";
            return RedirectToAction("AuctionPage", "Auction", new { aid = _review.AuctionId });
        }
    }
}
