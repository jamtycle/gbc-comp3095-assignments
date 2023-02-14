using System.ComponentModel.DataAnnotations;
using System.Data;
using assignment1.Models.Generics;

namespace assignment1.Models.Auth
{
    public class LoginModel : UserBase
    {
        public LoginModel() : base() { }

        public LoginModel(DataRow _row) : base(_row) { }

        [Required]
        public override string Username { get; set; }
        [Required]
        public override string Password { get; set; }   
        [Required]
        public bool RememberMe { get; set; }
    }
}