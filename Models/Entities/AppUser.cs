using Microsoft.AspNetCore.Identity;

namespace RYT.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

        // navigation props
        //public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public Wallet? Wallet { get; set; }
        public string? NameofSchool { get; set; }
    }
}
