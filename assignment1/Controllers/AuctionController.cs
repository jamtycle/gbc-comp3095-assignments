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
            if (!_aid.HasValue) return View();

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
        public IActionResult PlaceBid(BidModel _bid)
        {
            UserBase user = this.RecoverUserSession();
            LayoutModel<BidModel> model = new() { User = user, Menus = this.GetMenus(user), Data = _bid };

            if (!ModelState.IsValid) return View(model);

            if (new DBConnector().AddBid(_bid))
                return Auction(_bid.AuctionId);

            return View(model);
        }

        [HttpPost("NewAuction")]
        public IActionResult NewAuction(LayoutModel<AuctionModel> _auction)
        {
            if (!Request.Cookies.ContainsKey(Persistent.UserSession_Cookie)) return RedirectToAction("Index", "Home");

            /*_auction.Data.Image = Request.Form.Files.FirstOrDefault().CopyTo();*/
            if (!ModelState.IsValid) return View(_auction);

            _auction.Data.Image = GetImageFromRequest("auction_pic");
            UserBase user = this.RecoverUserSession();

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