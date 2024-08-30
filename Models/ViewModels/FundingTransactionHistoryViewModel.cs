namespace RYT.Models.ViewModels
{
    public class FundingTransactionHistoryViewModel
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
