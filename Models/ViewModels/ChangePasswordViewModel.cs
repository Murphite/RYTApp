using System.ComponentModel.DataAnnotations;

namespace RYT.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage ="New Password is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirmation Password is required")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
