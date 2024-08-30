using System.ComponentModel.DataAnnotations;

namespace RYT.Models.ViewModels
{
    public class LoginViewModel
    {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;


            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = string.Empty;
        
    }

}

