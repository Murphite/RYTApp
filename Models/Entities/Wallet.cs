using RYT.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace RYT.Models.Entities
{
    public class Wallet:BaseEntity
    {
        public string UserId { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }
        public WalletStatus Status { get; set; } = WalletStatus.Active;

        // Navigation Props
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public AppUser? User { get; set; }
    }
}
