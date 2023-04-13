using assignment1.Models.Auction;

namespace assignment1.Models.Home
{
    public class SearchModel
    {
        public string SearchText { get; set; }
        public string ConditionText { get; set; } = "All";
        public double MinPrice { get; set; } = 0;
        public double MaxPrice { get; set; } = 1000;
        public string Status { get; set; } = "all";
        public IEnumerable<AuctionModel> Search { get; set; }
    }
}