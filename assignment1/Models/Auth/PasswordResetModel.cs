using System.ComponentModel.DataAnnotations;

namespace assignment1.Models.Auth
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "The New Password field is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "The Confirm New Password field is required.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm New Password")]
        public string ConfirmPassword { get; set; }
        
        [Required]
        public string Key { get; set; }
    }
}