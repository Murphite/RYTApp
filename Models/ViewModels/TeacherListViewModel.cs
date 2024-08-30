using RYT.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RYT.Models.ViewModels
{
    public class TeacherListViewModel
    {
        public IList<Teacher>? TeacherList { get; set; } = new List<Teacher>();
        public string SchoolName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string YearsOfTeaching {  get; set; } = string.Empty;
        public int PageSize { get; set; } = 10;
        public int CurrentPage { get; set; } 
        public int TotalPages { get; set; }
        public int Count { get; set; }
        public int SchoolId { get; set; }
        public RewardToSendViewModel? RewardToSend { get; set; }

        public string CurrentSchool { get; set; }



        // Additional properties for search and pagination
        public string SearchKeyword { get; set; } = string.Empty;
        public string SearchCriteria { get; set; } = "All";

        public bool PreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool NextPage
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }

    }
}
public class RewardToSendViewModel
{
    public string userId { get; set; }
    [Required]
    public decimal Amount { get; set; }
}