using assignment1.Models.Auction;

namespace assignment1.Models.Home
{
    public class SearchModel
    {
        public string SearchText { get; set; }
        public IEnumerable<AuctionModel> Search { get; set; }
    }
}