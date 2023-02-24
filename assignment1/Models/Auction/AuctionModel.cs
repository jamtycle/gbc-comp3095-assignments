using System.Data;
using System.ComponentModel.DataAnnotations;
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

        public AuctionModel() : base()
        {

        }

        public AuctionModel(DataRow _auction) : base(_auction)
        {

        }

        public AuctionModel(DataRow _auction, DataTable _bids) : base(_auction)
        {
            bids = BidModel.BuildBids(_bids);
        }

        public int Auction_id { get => auction_id; set => auction_id = value; }
        
        public int User_id { get => user_id; set => user_id = value; }
        public string Auction_name { get => auction_name; set => auction_name = value; }
        [Required]
        public double Start_price { get => start_price; set => start_price = value; }
        [Required]
        public double Buy_now_price { get => buy_now_price; set => buy_now_price = value; }
        [Required]
        public DateTime Start_date { get => start_date; set => start_date = value; }
        [Required]
        public DateTime End_date { get => end_date; set => end_date = value; }

        public float Comission { get => comission; set => comission = value; }
        public float Tax { get => tax; set => tax = value; }
        public float Discount_percentage { get => discount_percentage; set => discount_percentage = value; }

        public IEnumerable<BidModel> Bids { get => bids; set => bids = value; }

        public byte[] Image { get => image; set => image = value;}
    }
}