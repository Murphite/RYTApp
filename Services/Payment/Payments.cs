using Microsoft.EntityFrameworkCore;
using RYT.Models.Entities;
using RYT.Models.Enums;
using RYT.Models.ViewModels;
using RYT.Services.PaymentGateway;
using RYT.Services.Repositories;
using RYT.Utilities;

namespace RYT.Services.Payment
{
    public class Payments : IPayments
    {
        private readonly IPaymentService _paymentService;
        private readonly IRepository _repository;
        public Payments(IPaymentService paymentService, IRepository repository)
        {
            _paymentService = paymentService;
            _repository = repository;
        }
        public async Task<Tuple<bool, string>> Initialize(FundWalletVM model, string userId)
        {
            var response = await _paymentService.InitializePayment(model, userId);

            if (!response.Item1) return new Tuple<bool, string>(response.Item1, response.Item2);
            
            var walletId = (await (await _repository.GetAsync<Wallet>())
                .FirstAsync(w => w.UserId == userId))
                .Id;
            
            var transaction = new Transaction
            {
                Amount = model.Amount,
                SenderId = userId,
                ReceiverId = "",
                WalletId = walletId,
                Reference = response.Item3,
                Status = false,
                Description = "Funding wallet",
                TransactionType = TransactionTypes.Funding.ToString(),
            };
            
            await _repository.AddAsync(transaction);
            var url = response.Item2;
            return new Tuple<bool, string>(true, url);
        }

        public async Task<bool> Verify(string reference)
        {
            var isSuccessful = await _paymentService.Verify(reference);
            
            if(!isSuccessful) return false;
            
            var transaction = await (await _repository.GetAsync<Transaction>())
                .FirstAsync(t => t.Reference == reference);
            
            transaction.Status = true;
            await _repository.UpdateAsync(transaction);
            
            var wallet = await (await _repository.GetAsync<Wallet>())
                .FirstAsync(w => w.Id == transaction.WalletId);
            
            wallet.Balance += transaction.Amount;
            await _repository.UpdateAsync(wallet);

            return true;
        }

        public async Task<IEnumerable<Bank>> GetBanks()
        {
            return await _paymentService.GetListOfBanks();
        }
        
        public async Task<bool> Withdraw(CreateWithdrawalVM model, string userId)
        {
            var response = await _paymentService.Withdraw(model);
            
            if (!response.Item1) return false;
            
            var senderWallet = await (await _repository.GetAsync<Wallet>())
                .FirstAsync(w => w.UserId == userId);
            
            if (senderWallet.Balance < model.Amount) 
                throw new InvalidOperationException("Insufficient funds");
            
            var transaction = new Transaction
            {
                Amount = model.Amount,
                SenderId = userId,
                ReceiverId = "",
                WalletId = senderWallet.Id,
                Reference = response.Item2,
                Status = true,
                Description = "Withdrawal",
                TransactionType = TransactionTypes.Withdrawal.ToString(),
            };
            
            await _repository.AddAsync(transaction);
            
            senderWallet.Balance -= model.Amount;
            await _repository.UpdateAsync(senderWallet);

            return true;
        }

        public async Task<bool> Transfer(string senderId, string receiverId, decimal amount)
        {
            var senderWallet = (await (await _repository.GetAsync<Wallet>())
                .FirstAsync(w => w.UserId == senderId));
            
            if (senderWallet.Balance < amount) 
                throw new InvalidOperationException("Insufficient funds");
            
            var receiverWallet = (await (await _repository.GetAsync<Wallet>())
                .FirstAsync(w => w.UserId == receiverId));
            var receiver = await _repository.GetAsync<AppUser>(receiverId);
            
            var transaction = new Transaction
            {
                Amount = amount,
                SenderId = senderId,
                ReceiverId = receiverId,
                WalletId = senderWallet.Id,
                Reference = TransactionHelper.GenerateTransRef(),
                Status = true,
                Description = $"Rewarding {receiver?.FirstName} {receiver?.LastName}",
                TransactionType = TransactionTypes.Transfer.ToString(),
            };
            
            await _repository.AddAsync(transaction);
            
            senderWallet.Balance -= amount;
            await _repository.UpdateAsync(senderWallet);
            
            receiverWallet.Balance += amount;
            await _repository.UpdateAsync(receiverWallet);

            return true;
        }
    }
}
