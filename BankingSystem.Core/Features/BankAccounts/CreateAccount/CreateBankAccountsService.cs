using BankingSystem.Core.Features.BankAccounts.Requests;
using BankingSystem.Core.Features.Transactions.TransactionsRepositories;

namespace BankingSystem.Core.Features.BankAccounts.CreateAccount;

public interface ICreateBankAccountsService
{
    Task<bool> CheckAccountOwnershipAsync(int accountId, string userId);
    Task<List<int>> CreateBankAccount(CreateBankAccountRequest createBankAccountRequest);
    Task<bool> ValidateAccountAsync(int accountId);
}

public class CreateBankAccountsService : ICreateBankAccountsService
{

    private readonly ICreateBankAccountsRepository _createBankAccountsRepository;
    private readonly ITransactionRepository _transactionRepository;

    public CreateBankAccountsService(ICreateBankAccountsRepository createBankAccountsRepository, ITransactionRepository transactionRepository)
    {
        _createBankAccountsRepository = createBankAccountsRepository;
        _transactionRepository = transactionRepository;

    }

    public async Task<List<int>> CreateBankAccount(CreateBankAccountRequest createBankAccountRequest)
    {
        var iban = IbanGenerator.GenerateIban();

        var currencies = Enum.GetValues<CurrencyType>();
        var accountIds = new List<int>();
        foreach (var currency in currencies)
        {
            BankAccount bankAccount = new BankAccount
            {
                UserId = createBankAccountRequest.UserId,
                Iban = iban,
                Currency = currency
            };

            var accountId = await _createBankAccountsRepository.CreateBankAccountAsync(bankAccount);
            accountIds.Add(accountId);

        }

        return accountIds;
    }

    public async Task<bool> CheckAccountOwnershipAsync(int accountId, string userId)
    {
        var isCorrectAccount = await _transactionRepository.CheckAccountOwnershipAsync(accountId, userId);
        if (!isCorrectAccount)
        {
            throw new Exception("You do not have permission to access this account.");
        }

        return isCorrectAccount;
    }

    public async Task<bool> ValidateAccountAsync(int accountId)
    {
        return await _createBankAccountsRepository.ContainsAccountAsync(accountId);
    }


}
