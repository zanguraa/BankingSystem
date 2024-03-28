using BankingSystem.Core.Features.BankAccounts.AddFunds.Models.Requests;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Core.Shared.Models;

namespace BankingSystem.Core.Features.BankAccounts.AddFunds;

public interface IAddFundsService
{
    Task<bool> AddFunds(AddFundsRequest addFundsRequest);
}

public class AddFundsService : IAddFundsService
{
    private readonly IAddFundsRepository _addFundsRepository;

    public AddFundsService(IAddFundsRepository addFundsRepository)
    {
        _addFundsRepository = addFundsRepository;
    }

    public async Task<bool> AddFunds(AddFundsRequest addFundsRequest)
    {
        ValidateAddFundsRequest(addFundsRequest);

        var transaction = new Transaction
        {
            ToAccountId = addFundsRequest.BankAccountId,
            ToAmount = addFundsRequest.Amount,
            TransactionType = (int)TransactionType.AddFunds,
            TransactionDate = DateTime.UtcNow
        };

        return await _addFundsRepository.ProcessDepositTransactionAsync(transaction);
    }

    private void ValidateAddFundsRequest(AddFundsRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "The request cannot be null.");
        }

        if (request.Amount <= 0)
        {
            throw new InvalidAddFundsValidationException("The amount must be greater than zero.");
        }

        if (request.BankAccountId <= 0)
        {
            throw new InvalidAddFundsValidationException("The Bank Account ID must be a positive number.");
        }
    }
}
