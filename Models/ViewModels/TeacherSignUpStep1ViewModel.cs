using RYT.Data;
using System.ComponentModel.DataAnnotations;

namespace RYT.Models.ViewModels
{
    public class TeacherSignUpStep1ViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string FirstName
        {
            get
            {
                string[] value = Name.Split(' ');
                return value.Length > 0 ? value[0] : string.Empty;
            }
        }

        public string LastName
        {
            get
            {
                string[] value = Name.Split(' ');
                return value.Length > 1 ? value[1] : string.Empty;
            }
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a school")]
        [Display(Name = "Schools where you taught")]
        public string SelectedSchool { get; set; } = string.Empty;

        public IList<string> SchoolsTaught { get; set; } = new List<string>();
    }
}
