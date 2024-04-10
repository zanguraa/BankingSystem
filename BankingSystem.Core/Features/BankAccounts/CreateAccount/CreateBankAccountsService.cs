using BankingSystem.Core.Features.BankAccounts.CreateAccount.Models.Requests;
using BankingSystem.Core.Features.Users.AuthorizeUser;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Core.Shared.Models;

namespace BankingSystem.Core.Features.BankAccounts.CreateAccount;

public interface ICreateBankAccountsService
{
    Task<List<string>> CreateBankAccountAsync(CreateBankAccountRequest createBankAccountRequest);
}

public class CreateBankAccountsService : ICreateBankAccountsService
{

    private readonly ICreateBankAccountsRepository _createBankAccountsRepository;
    private readonly IAuthorizeUserRepository _authorizeUserRepository;
    private readonly ISeqLogger _seqLogger;


    public CreateBankAccountsService(ICreateBankAccountsRepository createBankAccountsRepository, IAuthorizeUserRepository authorizeUserRepository, ISeqLogger seqLogger)
    {
        _createBankAccountsRepository = createBankAccountsRepository;
        _authorizeUserRepository = authorizeUserRepository;
        _seqLogger = seqLogger;
    }

    public async Task<List<string>> CreateBankAccountAsync(CreateBankAccountRequest createBankAccountRequest)
    {
        await ValidateUserDoesNotHaveAccountAsync(createBankAccountRequest.UserId);

        var ibans = new List<string>();
        var accountIds = new List<int>();
        var currencies = Enum.GetValues<Currency>();
        foreach (var currency in currencies)
        {
            string iban = IbanGenerator.GenerateIban();
            BankAccount bankAccount = new()
            {
                UserId = createBankAccountRequest.UserId,
                Iban = iban,
                Currency = currency
            };

            var accountId = await _createBankAccountsRepository.CreateBankAccountAsync(bankAccount);
            accountIds.Add(accountId);
        }

        if (accountIds.Count > 0)
        {
            ibans = await _createBankAccountsRepository.GetIbansByAccountIdsAsync(accountIds);
        }

        _seqLogger.LogInfo("Accounts for user: {userId} has been created successfully. Ibans: {Ibans}.", createBankAccountRequest.UserId, ibans);

        return ibans;
    }

    private async Task ValidateUserDoesNotHaveAccountAsync(int userId)
    {
        bool userExists = await _authorizeUserRepository.UserExistsAsync(userId);
        if (!userExists)
        {
            throw new UserNotFoundException($"user ID {userId} is not exists.");
        }
        if (userId == 0)
        {
            throw new UserNotFoundException($"user ID {userId} is not valid.");
        }
    }
}
