using System.ComponentModel.DataAnnotations;
using System.Data;
using assignment1.Models.Generics;

namespace assignment1.Models.Auth
{
    public class RegistrationModel : UserBase
    {
        public RegistrationModel() : base() { }

        public RegistrationModel(DataRow _row) : base(_row) { }

        public override int UserTypeId { get; set; } = 1;
        [Required]
        public override string Username { get; set; }
        [Required]
        public override string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords missmatch!")]
        public string VerifyPassword { get; set; }
        [Required]
        public override string FirstName { get => base.FirstName; set => base.FirstName = value; }
        [Required]
        public override string LastName { get => base.LastName; set => base.LastName = value; }
        [Required]
        public override string Email { get => base.Email; set => base.Email = value; }
        [Required]
        public override DateTime DateOfBirth { get => base.DateOfBirth; set => base.DateOfBirth = value; }
    }
}