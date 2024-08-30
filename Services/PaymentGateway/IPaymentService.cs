using RYT.Models.ViewModels;

namespace RYT.Services.PaymentGateway
{
    public interface IPaymentService
    {
        public Task<Tuple<bool, string, string>> InitializePayment(FundWalletVM model, string userId);
        public Task<Tuple<bool, string>> Withdraw(CreateWithdrawalVM model);
        public Task<bool> Verify(string reference);

        public Task<IEnumerable<Bank>> GetListOfBanks();
    }
}