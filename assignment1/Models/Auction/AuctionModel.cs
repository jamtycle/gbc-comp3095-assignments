using System.ComponentModel.DataAnnotations;
using System.Data;
using assignment1.Models.Generics;

namespace assignment1.Models.Auction
{
    public class AuctionModel : ModelBase
    {
        private int auction_id;
        private int user_id;

        private string auction_name;
        private double start_price;
        private double buy_now_price;
        private DateTime start_date;
        private DateTime end_date;

        private float comission;
        private float tax;
        private float discount_percentage;

        private IEnumerable<BidModel> bids;

        private byte[] image;
        private string condition;
        private string description;

        private bool? has_been_buyed = null;

        private string reviewers = string.Empty;

        public AuctionModel() : base() { }

        public AuctionModel(DataRow _auction) : base(_auction)
        {

        }

        public AuctionModel(DataRow _auction, DataTable _bids) : base(_auction)
        {
            bids = BidModel.BuildBids(_bids);
        }

        public int AuctionId { get => auction_id; set => auction_id = value; }

        [Required]
        public int UserId { get => user_id; set => user_id = value; }
        [Required]
        public string AuctionName { get => auction_name; set => auction_name = value; }
        [Required]
        public double StartPrice { get => start_price; set => start_price = value; }
        [Required]
        public double BuyNowPrice { get => buy_now_price; set => buy_now_price = value; }
        [Required]
        public DateTime StartDate { get => start_date; set => start_date = value; }
        [Required]
        public DateTime EndDate { get => end_date; set => end_date = value; }

        public float Comission { get => comission; set => comission = value; }
        public float Tax { get => tax; set => tax = value; }
        public float DiscountPercentage { get => discount_percentage; set => discount_percentage = value; }

        public IEnumerable<BidModel> Bids { get => bids; set => bids = value; }

        public byte[] Image { get => image; set => image = value; }
        [Required] 
        public string Condition { get => condition; set => condition = value; }
        public string Description { get => description; set => description = value; }

        public bool HasBeenBuyedNow { get => has_been_buyed ?? Bids?.FirstOrDefault(x => x.BuyedNow) != null; }
        public BidModel GetBuyedNow { get => Bids?.FirstOrDefault(x => x.BuyedNow); }
        public bool IsActive { get => !HasBeenBuyedNow && DateTime.UtcNow <= EndDate; }

        public double BidStart
        {
            get 
            {
                var mbid = bids.Max();
                return (mbid == null ? StartPrice : mbid.BidAmount < StartPrice ? StartPrice : mbid.BidAmount) + 0.1;
            }
        }

        public BidModel LastBid { get => bids.Max(); }
        public IEnumerable<int> Reviewers { get => reviewers?.Split(',').Select(x => {
            if (int.TryParse(x, out int res)) return res;
            else return -1;
        }); }
    }
}