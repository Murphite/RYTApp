namespace RYT.Models.Entities
{
    public class UserChat: BaseEntity
    {
        public string Text { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;
        public DateTime DeliveredOn { get; set; } = DateTime.UtcNow;
        public string ThreadId { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
        public DateTime ReadOn { get; set; } = DateTime.UtcNow;
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        // Navigation prop
        public AppUser? Sender { get; set; }

    }
}
