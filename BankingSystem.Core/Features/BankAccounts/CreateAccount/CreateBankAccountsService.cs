using BankingSystem.Core.Features.BankAccounts.Requests;
using BankingSystem.Core.Features.Transactions.TransactionsRepositories;
using BankingSystem.Core.Shared.Exceptions;

namespace BankingSystem.Core.Features.BankAccounts.CreateAccount;

public interface ICreateBankAccountsService
{
    Task<List<int>> CreateBankAccount(CreateBankAccountRequest createBankAccountRequest);
}

public class CreateBankAccountsService : ICreateBankAccountsService
{

    private readonly ICreateBankAccountsRepository _createBankAccountsRepository;

    public CreateBankAccountsService(ICreateBankAccountsRepository createBankAccountsRepository)
    {
        _createBankAccountsRepository = createBankAccountsRepository;
    }

    public async Task<List<int>> CreateBankAccount(CreateBankAccountRequest createBankAccountRequest)
    {
        await ValidateUserDoesNotHaveAccount(createBankAccountRequest.UserId);

        bool accountExists = await _createBankAccountsRepository.ContainsAccountForUserAsync(createBankAccountRequest.UserId);
        if (accountExists)
        {
            throw new InvalidOperationException("An account for this user already exists.");
        }

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

    private async Task ValidateUserDoesNotHaveAccount(int userId)
    {
        bool accountExists = await _createBankAccountsRepository.ContainsAccountForUserAsync(userId);
        if (accountExists)
        {
            throw new BankAccountsAlreadyExistsException($"An account for user ID {userId} already exists.");
        }
    }

}
