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
        private string username;
        private DateTime bid_date;
        private double bid_amount;
        private bool buyed_now = false;

        public BidModel() : base()
        {

        }

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
        public DateTime BidDate { get => bid_date; set => bid_date = value; }
        [Required]
        public double BidAmount { get => bid_amount; set => bid_amount = value; }
        public string Username { get => username; set => username = value; }
        public bool BuyedNow { get => buyed_now; set => buyed_now = value; }
        public string TimeMark
        {
            get
            {
                var tdiff = DateTime.Now - BidDate;
                if (tdiff.TotalDays >= 365)
                    return $"{tdiff.TotalDays / 365:###} years ago";
                else if (tdiff.TotalDays >= 1)
                    return $"{tdiff.TotalDays:###} days ago";
                else if (tdiff.TotalHours >= 1)
                    return $"{tdiff.TotalHours:##} hours ago";
                else if (tdiff.TotalMinutes >= 1)
                    return $"{tdiff.TotalMinutes:##} minutes ago";
                else
                    return $"{tdiff.TotalSeconds:##} seconds ago";
            }
        }
    }
}