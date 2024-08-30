using System.ComponentModel.DataAnnotations;

namespace RYT.Models.ViewModels
{
    public class StudentSignUpViewModel
    {
        //[Required]
        ////public string FirstName { get; set; } = string.Empty;
        //[Required]
        ////public string LastName { get; set; } = string.Empty;
        [Required]
        public string? FullName {  get; set; } = string.Empty;

        public string FirstName { get
            {
                var firstname = FullName.Trim().Split(" "); 
                return firstname[0];
            } }
        public string LastName
        {
            get
            {
                var lastName = FullName.Trim().Split(" ");
                return lastName.Length > 1 ? lastName[1] : string.Empty;
            }
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
