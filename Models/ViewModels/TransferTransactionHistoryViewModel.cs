namespace RYT.Models.ViewModels
{
    public class TransferTransactionHistoryViewModel
    {
        public string NameOfTeacher { get; set; } = string.Empty;
        public string School { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime dateTime { get; set; }
    }
}
