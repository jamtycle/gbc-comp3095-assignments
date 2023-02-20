using System.ComponentModel.DataAnnotations;
using System.Data;
using assignment1.Models.Generics;

namespace assignment1.Models.Auth
{
    public class RegistrationModel : UserBase
    {
        private int user_type_id = 1;
        private string username;
        private string password;
        private string first_name;
        private string last_name;
        private string email;
        private DateTime date_of_birth;

        public RegistrationModel() : base() { }

        public RegistrationModel(DataRow _row) : base(_row) { }

        public override int UserTypeId { get => user_type_id; set => user_type_id = value; }
        [Required]
        public override string Username { get => username; set => username = value; }
        [Required]
        public override string Password { get => password; set => password = value; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords missmatch!")]
        public string VerifyPassword { get; set; }
        [Required]
        public override string FirstName { get => first_name; set => first_name = value; }
        [Required]
        public override string LastName { get => last_name; set => last_name = value; }
        [Required]
        public override string Email { get => email; set => email = value; }
        [Required]
        public override DateTime DateOfBirth { get => date_of_birth; set => date_of_birth = value; }
    }
}