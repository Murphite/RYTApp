namespace RYT.Models.ViewModels
{
    public class OverviewViewModel
    {
        public decimal Balance { get; set; }
        public decimal AmountSent { get; set; }
        public decimal AmountReceived { get; set; }
        public string Status { get; set; }
        public List<FundingTransactionHistoryViewModel> MyFundings { get; set; } = new List<FundingTransactionHistoryViewModel>();
        public List<ReceivedTransactionsViewModel> MyReceivedTransactions { get; set; } = new List<ReceivedTransactionsViewModel>();

        public FundWalletVM FundingVM { get; set; }
    }
}
