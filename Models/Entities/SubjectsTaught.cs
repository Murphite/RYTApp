namespace RYT.Models.Entities
{
    public class SubjectsTaught : BaseEntity
    {
        public string TeacherId { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;

        // navigation prop
        public Teacher? Teacher { get; set; }
    }
}
