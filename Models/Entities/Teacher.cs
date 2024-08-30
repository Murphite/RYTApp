namespace RYT.Models.Entities
{
    public class Teacher : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string YearsOfTeaching { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public IList<SubjectsTaught> TeacherSubjects { get; set; } = new List<SubjectsTaught>();
        public IList<SchoolsTaught> SchoolsTaughts { get; set; } = new List<SchoolsTaught>();

        public string SchoolType { get; set; } = string.Empty;
        public string NINUploadUrl { get; set; } = string.Empty;
        public string NINUploadPublicId { get; set; } = string.Empty;

        // navigation property
        public AppUser? User { get; set; }
    }
}
