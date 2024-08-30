namespace RYT.Models.ViewModels
{
    public class ListOfSchoolViewModel
    {
        public List<string> Schools { get; set; } = new List<string>();
        public string SchoolName { get; set; } = string.Empty;
        public string SchoolType { get; set; } = string.Empty;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int Count { get; set; }
        public int PageSize { get; set; }


    }
}
