using RYT.Models.ViewModels;

namespace RYT.Services.Payment;

public interface IPayments
{
    public Task<Tuple<bool, string>> Initialize(FundWalletVM model, string userId);
    public Task<bool> Verify(string reference);
    public Task<IEnumerable<Bank>> GetBanks();
    public Task<bool> Withdraw(CreateWithdrawalVM model, string userId);
    public Task<bool> Transfer(string senderId, string receiverId, decimal amount);
}