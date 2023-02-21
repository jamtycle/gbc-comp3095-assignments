using assignment1.Data;
using assignment1.Models;
using assignment1.Models.Auction;
using assignment1.Models.Generics;
using Microsoft.AspNetCore.Mvc;

namespace assignment1.Controllers
{
    public class AuctionController : BaseController
    {
        public AuctionController(ILogger<AuctionController> _logger) : base(_logger) { }

        [HttpGet("Auction")]
        public IActionResult Auction([FromQuery(Name = "aid")] int? _aid)
        {
            if (!_aid.HasValue) return View();

            AuctionModel auct = new DBConnector().GetAuction(_aid.Value);
            UserBase user = this.RecoverUserSession();

            LayoutModel<AuctionModel> model = new()
            {
                Menus = this.GetMenus(user),
                Data = auct
            };

            return View(model);
        }

        [HttpPost("PlaceBid")]
        public IActionResult PlaceBid(BidModel _bid)
        {
            UserBase user = this.RecoverUserSession();
            LayoutModel<BidModel> model = new() { Menus = this.GetMenus(user), Data = _bid };

            if (!ModelState.IsValid) return View(model);

            if (new DBConnector().AddBid(_bid))
                return Auction(_bid.Auction_id);

            return View(model);
        }
    }
}