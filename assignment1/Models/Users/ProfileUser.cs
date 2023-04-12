using System.ComponentModel.DataAnnotations;
using System.Data;
using assignment1.Libs;
using assignment1.Models.Generics;

namespace assignment1.Models.Users
{
    public class ProfileUser : UserBase
    {
        public string username;
        public string password; 

        public ProfileUser() : base() { }

        public ProfileUser(DataRow _row) : base(_row) { }
        
        public ProfileUser(UserBase _base) 
        {
            this.Id = _base.Id;
            this.username = _base.Username;
            this.FirstName = _base.FirstName;
            this.LastName = _base.LastName;
            this.DateOfBirth = _base.DateOfBirth;
            this.TwoFactorAuth = _base.TwoFactorAuth;
            this.ProfilePic = _base.ProfilePic;
        }

        [Required]
        public override int Id { get => base.Id; set => base.Id = value; }
        [Required]
        public override string Username { get => username; set => username = value; }
        public override string Password { get => password; set => password = value; }

        [Required]
        public override string FirstName { get => base.FirstName; set => base.FirstName = value; }
        [Required]
        public override string LastName { get => base.LastName; set => base.LastName = value; }
        [Required]
        public override DateTime DateOfBirth { get => base.DateOfBirth; set => base.DateOfBirth = value; }
        [Required]
        public override bool TwoFactorAuth { get => base.TwoFactorAuth; set => base.TwoFactorAuth = value; }
        public override byte[] ProfilePic { get => base.ProfilePic; set => base.ProfilePic = value; }
    }
}
