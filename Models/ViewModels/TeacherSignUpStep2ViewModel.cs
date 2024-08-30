using System.ComponentModel.DataAnnotations;

namespace RYT.Models.ViewModels
{
    public class TeacherSignUpStep2ViewModel
    {
        // [Required]
        // [Display(Name = "Name")]
        // public string Name { get; set; } = string.Empty;
        //
        // public string FirstName
        // {
        //     get
        //     {
        //         string[] value = Name.Split(' ');
        //         return value.Length > 0 ? value[0] : string.Empty;
        //     }
        //     set { }
        // }
        //
        // public string LastName
        // {
        //     get
        //     {
        //         string[] value = Name.Split(' ');
        //         return value.Length > 1 ? value[1] : string.Empty;
        //     }
        //     set { }
        // }
        //
        // [Required]
        // [EmailAddress]
        // [Display(Name = "Email")]
        // public string Email { get; set; } = string.Empty;
        //
        // [Required]
        // [DataType(DataType.Password)]
        // [Display(Name = "Password")]
        // public string Password { get; set; } = string.Empty;
        //
        // [Required(ErrorMessage = "Please select a school")]
        // [Display(Name = "Schools where you taught")]
        // public string SelectedSchool { get; set; } = string.Empty;
        //
        // public IList<string> SchoolsTaught { get; set; } = new List<string>();


        [Required]
        [Display(Name = "Years of Teaching")]
        [RegularExpression(@"\d{4} - \d{4}", ErrorMessage = "Invalid format. Please use the format 'YYYY - YYYY'.")]
        public string YearsOfTeaching { get; set; } = string.Empty;

        // Calculated years
        public string CalculatedYearsOfTeaching
        {
            get
            {
                if (!string.IsNullOrEmpty(YearsOfTeaching))
                {
                    var years = YearsOfTeaching.Split('-');
                    if (years.Length == 2 && int.TryParse(years[0].Trim(), out int startYear) && int.TryParse(years[1].Trim(), out int endYear))
                    {
                        return (endYear - startYear + 1).ToString();
                    }
                }
                return string.Empty;
            }
            set { }
        }

        [Required(ErrorMessage = "Please select at least a Subject")]
        [Display(Name = "Subjects Taught")]
        public string SelectedSubject { get; set; } = string.Empty;

        public IList<string> listOfSubjectsTaught { get; set; } = new List<string>();

        [Required(ErrorMessage = "Please select a School Type")]
        [Display(Name = "School Type")]
        public string SelectedSchoolType { get; set; } = string.Empty;

        public IList<string> ListOfSchoolTypes { get; set; } = new List<string>();

        public IFormFile? NINUploadImage { get; set; }
        
        /*
        public string NINUploadPublicId { get; set; } = string.Empty;
        public string FolderName { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;*/

    }
}
