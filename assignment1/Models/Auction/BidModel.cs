using System.ComponentModel.DataAnnotations;
using System.Data;
using assignment1.Models.Generics;

namespace assignment1.Models.Auction
{
    public class BidModel : ModelBase
    {
        private int id;
        private int auction_id;
        private int user_id;
        private DateTime bid_date;
        private double bid_amount;

        public BidModel(DataRow _row) : base(_row)
        {

        }

        public static IEnumerable<BidModel> BuildBids(DataTable _bids)
        {
            foreach (DataRow row in _bids.Rows)
                yield return new BidModel(row);
        }

        public int Id { get => id; set => id = value; }
        [Required]
        public int Auction_id { get => auction_id; set => auction_id = value; }
        [Required]
        public int User_id { get => user_id; set => user_id = value; }
        [Required]
        public DateTime Bid_date { get => bid_date; set => bid_date = value; }
        [Required]
        public double Bid_amount { get => bid_amount; set => bid_amount = value; }
    }
}