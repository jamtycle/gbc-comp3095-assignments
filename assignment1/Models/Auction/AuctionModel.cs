using assignment1.Models.Generics;

namespace assignment1.Models.Auction
{
    public class AuctionModel : ModelBase
    {
        public IEnumerable<BidModel> Bids { get; set; }
    }
}