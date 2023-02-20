using System.ComponentModel.DataAnnotations;
using System.Data;
using assignment1.Models.Generics;

namespace assignment1.Models.Auth
{
    public class LoginModel : UserBase
    {
        private string username;
        private string password;
        private string session;

        public LoginModel() : base() { }

        public LoginModel(DataRow _row) : base(_row) { }

        [Required]
        public override string Username { get => username; set => username = value; }
        [Required]
        public override string Password { get => password; set => password = value; }
        [Required]
        public bool RememberMe { get; set; }
        public override string SessionCookie { get => session; set => session = value; }
    }
}