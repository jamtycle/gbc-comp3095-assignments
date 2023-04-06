using assignment1.Data;
using assignment1.Models.Auction;
using assignment1.Models.Generics;
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
            return base.File(new DBConnector().GetAuctionPic(_id), "image/jpeg");
        }

        [HttpGet("UserImage")]
        public IActionResult UserImage([FromQuery(Name="id")] int _id)
        {
            return base.File(new DBConnector().GetUserPic(_id), "image/jpeg");
        }
    }
}