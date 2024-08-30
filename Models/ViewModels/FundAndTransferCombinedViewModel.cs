namespace RYT.Models.ViewModels
{
    public class FundAndTransferCombinedViewModel
    {
        public List<FundingTransactionHistoryViewModel> FundingTransactions { get; set; } = new List<FundingTransactionHistoryViewModel>();
        public List<TransferTransactionHistoryViewModel> TransferTransactions { get; set; } = new List<TransferTransactionHistoryViewModel>();
    }
}
