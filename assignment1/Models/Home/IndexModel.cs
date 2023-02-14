using assignment1.Models.Auction;
using assignment1.Models.Generics;

namespace assignment1.Models.Home
{
    public class IndexModel
    {
        public IEnumerable<AuctionModel> Carousel { get; set; }
        public UserBase User { get; set; }
        public IEnumerable<AuctionModel> AuctionList { get; set; }
    }
}