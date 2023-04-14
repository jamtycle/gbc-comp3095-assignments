using System.ComponentModel.DataAnnotations;
using System.Data;
using assignment1.Models.Generics;
namespace assignment1.Models
{
    public class ReviewModel : ModelBase
    {
        private int id;
        private int auction_id;
        private int user_id;
        private string user_username;
        private int reviewRating;
        private string[] review;

        public ReviewModel() : base() { }
        public ReviewModel(DataRow _row) : base(_row) { }

        [Required]
        public int Id { get => id; set => id = value; }
        [Required]
        public int AuctionId { get => auction_id; set => auction_id = value; }
        [Required]
        public int UserId { get => user_id; set => user_id = value; }
        [Required]
        public string UserUsername { get => user_username; set => user_username = value; }
        [Required]
        public int ReviewRating { get => reviewRating; set => reviewRating = value; }
        [Required]
        [Range(1, 300, ErrorMessage = "The review must be no longer than 300 characters")]
        public string[] Review { get => review; set => review = value; }
    }
}
