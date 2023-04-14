using System.ComponentModel.DataAnnotations;
using System.Data;
using assignment1.Models.Generics;

namespace assignment1.Models.Reviews
{
    public class ReviewModel : ModelBase
    {
        private int auction_id;
        private int user_id;
        private int user_rating_id;
        private string username;
        private string auction_name;
        private int rating;

        public ReviewModel() : base() { }

        public ReviewModel(DataRow _row) : base(_row) { }

        [Required]
        public int AuctionId { get => auction_id; set => auction_id = value; }
        [Required]
        public int UserId { get => user_id; set => user_id = value; }
        [Required]
        public int UserRatingId { get => user_rating_id; set => user_rating_id = value; }
        public string RatingUserUsername { get => username; set => username = value; }
        [Required]
        [Range(1, 5)]
        public int Rating { get => rating; set => rating = value; }
        public string AuctionName { get => auction_name; set => auction_name = value; }
    }
}
