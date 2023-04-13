using assignment1.Data;
using assignment1.Libs;
using assignment1.Models;
using assignment1.Models.Auction;
using assignment1.Models.Generics;
using Microsoft.AspNetCore.Mvc;

namespace assignment1.Controllers
{
    [Route("Auction")]
    public class AuctionController : BaseController
    {
        public AuctionController(ILogger<AuctionController> _logger) : base(_logger) { }

        [HttpGet("NewAuction")]
        public IActionResult NewAuction()
        {
            if (!Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return RedirectToAction("Index", "Home");

            UserBase user = this.RecoverUserSession();
            if (user == null) return Forbid();
            else if (user.UserTypeId != Persistent.user_type_table.FirstOrDefault(x => x.UserTypeName.Equals("Seller")).UserTypeId) return Forbid();

            return View(new LayoutModel<AuctionModel>()
            {
                User = user,
                Menus = this.GetMenus(user),
                Data = new AuctionModel()
                {
                    UserId = user.Id
                }
            });
        }

        [HttpGet("AuctionPage")]
        public IActionResult Auction([FromQuery(Name = "aid")] int? _aid)
        {
            if (!_aid.HasValue) return RedirectToAction("Index", "Home");
            if (TempData.ContainsKey("Error")) ViewBag.Error = TempData["Error"];

            AuctionModel auct = new DBConnector().GetAuction(_aid.Value);
            UserBase user = this.RecoverUserSession();

            LayoutModel<AuctionModel> model = new()
            {
                User = user,
                Menus = this.GetMenus(user),
                Data = auct
            };

            return View("Auction", model);
        }

        [HttpPost("PlaceBid")]
        public IActionResult PlaceBid(int auction_id, float bid_amt)
        {
            UserBase user = this.RecoverUserSession();
            if (user == null) return RedirectToAction("AuctionPage", "Auction", new { aid = auction_id }); // Auction(auction_id);

            BidModel bid = new()
            {
                AuctionId = auction_id,
                BidAmount = bid_amt,
                BidDate = DateTime.Now,
                UserId = user.Id
            };

            AuctionModel auction = new DBConnector().GetAuction(auction_id);
            if (!auction.IsActive)
            {
                TempData["Error"] = "The Auction is no longer active.";
                return RedirectToAction("AuctionPage", "Auction", new { aid = auction_id });
            }

            if (auction.LastBid?.BidAmount >= bid_amt)
            {
                TempData["Error"] = "The bid entered is less or equals than the actual bid.";
                return RedirectToAction("AuctionPage", "Auction", new { aid = auction_id });
            }

            if (auction.StartPrice > bid_amt)
            {
                TempData["Error"] = "The bid entered is less than the start price.";
                return RedirectToAction("AuctionPage", "Auction", new { aid = auction_id });
            }

            if (!new DBConnector().AddBid(bid))
                TempData["Error"] = "Something went wrong 🥲";

            return RedirectToAction("AuctionPage", "Auction", new { aid = auction_id });
        }

        [HttpPost("BuyNowAuction")]
        public IActionResult BuyNowAuction(int auction_id)
        {
            UserBase user = this.RecoverUserSession();
            if (user == null) return Auction(auction_id);

            AuctionModel auction = new DBConnector().GetAuction(auction_id);
            if (!auction.IsActive)
            {
                TempData["Error"] = "The Auction is no longer active.";
                return RedirectToAction("AuctionPage", "Auction", new { aid = auction_id });
            }

            BidModel bid = new()
            {
                AuctionId = auction_id,
                BidAmount = auction.BuyNowPrice,
                BidDate = DateTime.Now,
                UserId = user.Id,
                BuyedNow = true
            };

            if (auction.LastBid.BidAmount >= auction.BuyNowPrice)
            {
                TempData["Error"] = "You cannot buy now this product anymore";
                return RedirectToAction("AuctionPage", "Auction", new { aid = auction_id });
            }

            if (!new DBConnector().AddBid(bid))
                TempData["Error"] = "Something went wrong 🥲";

            return RedirectToAction("AuctionPage", "Auction", new { aid = auction_id });
        }

        [HttpPost("NewAuction")]
        public IActionResult NewAuction(LayoutModel<AuctionModel> _auction)
        {
            if (!Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return RedirectToAction("Index", "Home");

            UserBase user = this.RecoverUserSession();
            if (user == null) return Forbid();
            else if (user.UserTypeId != Persistent.user_type_table.FirstOrDefault(x => x.UserTypeName.Equals("Seller")).UserTypeId) return Forbid();

            if (!ModelState.IsValid) return View(_auction);

            _auction.Data.Image = GetImageFromRequest("auction_pic");

            LayoutModel<AuctionModel> model = new()
            {
                User = user,
                Menus = this.GetMenus(user),
                Data = new AuctionModel()
                {
                    UserId = user.Id
                }
            };

            if (!user.Id.Equals(_auction.Data.UserId)) return View(model);

            int nid = new DBConnector().NewAuction(_auction.Data);
            if (nid == -1)
                return View(model); //RedirectToAction("Index", "Home");

            return Auction(nid);
        }

        public byte[] GetImageFromRequest(string _name)
        {
            using var ms = new MemoryStream();
            Request.Form.Files[_name].CopyTo(ms);
            return ms.ToArray();
        }
    }
}