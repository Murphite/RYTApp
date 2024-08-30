namespace RYT.Models.ViewModels
{
    public class ReceivedTransactionsViewModel
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime timeOfTransaction { get; set; } = DateTime.UtcNow;
    }
}
