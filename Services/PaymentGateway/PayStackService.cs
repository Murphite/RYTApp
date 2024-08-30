using PayStack.Net;
using RYT.Data;
using RYT.Models.Entities;
using RYT.Models.ViewModels;
using RYT.Services.Repositories;
using RYT.Utilities;
using Bank = RYT.Models.ViewModels.Bank;

namespace RYT.Services.PaymentGateway;

public class PayStackService : IPaymentService
{
    private readonly IConfiguration _configuration;
    private readonly IRepository _repository;
    private readonly string _secretKey;
    private readonly PayStackApi _payStack;
    public string Url { get; set; }

    public PayStackService(IConfiguration configuration, RYTDbContext context, IRepository repository)
    {
        _configuration = configuration;
        _repository = repository;
        _secretKey = _configuration["Payment:PayStackSecretKey"];
        _payStack = new PayStackApi(_secretKey);
    }

    public async Task<Tuple<bool, string, string>> InitializePayment(FundWalletVM model, string userId)
    {
        var senderEmail = (await _repository.GetAsync<AppUser>())
            .Where(s => s.Id == userId)
            .Select(s => s.Email)
            .First();

        var transactionRef = TransactionHelper.GenerateTransRef();
            
        var request = new TransactionInitializeRequest
        {
            AmountInKobo = (int)model.Amount * 100,
            Email = senderEmail,
            Currency = "NGN",
            CallbackUrl = _configuration["Payment:PayStackCallbackUrl"],
            Reference = transactionRef,
        };
            
        var response = _payStack.Transactions.Initialize(request);
            
        if (!response.Status) return new Tuple<bool, string, string>(false, response.Message, transactionRef);
            
        return new Tuple<bool, string, string>(true, response.Data.AuthorizationUrl, transactionRef);
    }

    public async Task<bool> Verify(string reference)
    {
        var verifyResponse = _payStack.Transactions.Verify(reference);

        return verifyResponse.Status;
    }
        
    public async Task<Tuple<bool, string>> Withdraw(CreateWithdrawalVM model)
    {
        var result = _payStack.Post<ApiResponse<dynamic>, dynamic>("transferrecipient", new
        {
            type = "nuban",
            name = model.AccountName,
            account_number = model.AccountNumber,
            bank_code = model.BankCode,
            currency = "NGN",
        });
        
        if (!result.Status)
            throw new Exception("Unable to create transfer recipient");
        
        var recipientCode = result.Data.recipient_code;

        var transactionRef = TransactionHelper.GenerateTransRef();
        
        // we can't currently transfer from our paystack balance because of limitations on the account
        
        // var transferResult = _payStack.Post<ApiResponse<dynamic>, dynamic>("transfer", new
        // {
        //     source = "balance",
        //     amount = model.Amount,
        //     reference = transactionRef,    
        //     recipient = recipientCode,
        //     reason = "Withdrawal",
        // });

        return new Tuple<bool, string>(true, transactionRef);
    }

    public async Task<IEnumerable<Bank>> GetListOfBanks()
    {
        var result = _payStack.Get<ApiResponse<dynamic>>("bank?currency=NGN");

        if (!result.Status)
            throw new Exception("Unable to fetch banks");

        var banks = result.Data.ToObject<List<Bank>>();
            
        return banks;
    }
}