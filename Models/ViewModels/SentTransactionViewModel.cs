namespace RYT.Models.ViewModels
{
    public class SentTransactionViewModel
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime timeOfTransaction { get; set; } = DateTime.UtcNow;
    }
}
