using assignment1.Data;
using assignment1.Models.Auction;
using Microsoft.AspNetCore.Mvc;

namespace assignment1.Controllers
{
    [Route("Images")]
    public class ImagesController : BaseController
    {
        public ImagesController(ILogger<ImagesController> _logger) : base(_logger)
        {
        }

        [HttpGet("AuctionImage")]
        public IActionResult AuctionImage([FromQuery(Name="id")] int _id)
        {
            AuctionModel auction = new DBConnector().GetAuction(_id);
            return base.File(auction.Image ?? Array.Empty<byte>(), "image/jpeg");
        }

        public IActionResult UserImage([FromQuery(Name="id")] int _id)
        {
            // UserBase user = new DBConnector().GetUser(_id);
            // return base.File(user.)
            throw new NotImplementedException("This functionality is not implemented yet!");
        }
    }
}