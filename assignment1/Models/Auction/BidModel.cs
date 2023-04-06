using System.ComponentModel.DataAnnotations;
using System.Data;
using assignment1.Models.Generics;

namespace assignment1.Models.Auction
{
    public class BidModel : ModelBase, IComparable<BidModel>
    {
        private int id;
        private int auction_id;
        private int user_id;
        private string user_username;
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

        public int CompareTo(BidModel other)
        {
            return this.bid_amount.CompareTo(other.bid_amount);
        }

        public int Id { get => id; set => id = value; }
        [Required]
        public int AuctionId { get => auction_id; set => auction_id = value; }
        [Required]
        public int UserId { get => user_id; set => user_id = value; }
        [Required]
        public DateTime BidDate { get => bid_date; set => bid_date = value; }
        [Required]
        public double BidAmount { get => bid_amount; set => bid_amount = value; }
        public string UserUsername { get => user_username; set => user_username = value; }
    }
}