using BankingSystem.Core.Features.Transactions.Shared;
using BankingSystem.Core.Features.Transactions.Shared.Models.Requests;
using BankingSystem.Core.Shared.Exceptions;

namespace BankingSystem.Core.Features.Transactions
{
    public interface ITransactionServiceValidator
    {
        Task ValidateCreateTransactionRequest(CreateTransactionRequest request);
    }

    public class CreateTransactionServiceValidator : ITransactionServiceValidator
    {
        private readonly ICreateTransactionRepository _createtransactionRepository;

        public CreateTransactionServiceValidator(ICreateTransactionRepository transactionRepository)
        {
            _createtransactionRepository = transactionRepository;
        }

        public async Task ValidateCreateTransactionRequest(CreateTransactionRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Transaction request cannot be null.");
            }

            if (string.IsNullOrEmpty(request.UserId))
            {
                throw new UserValidationException("User not found.");
            }

            if (string.IsNullOrEmpty(request.UserId))
            {
                throw new InvalidTransactionValidation("User ID cannot be empty.");
            }

            if (request.FromAccountId <= 0)
            {
                throw new InvalidTransactionValidation("From Account ID must be positive.");
            }

            if (request.ToAccountId <= 0)
            {
                throw new InvalidTransactionValidation("To Account ID must be positive.");
            }

            if (request.Amount <= 0)
            {
                throw new InvalidTransactionValidation("Transaction amount must be greater than zero.");
            }

            bool fromCurrencyIsValid = await _createtransactionRepository.IsCurrencyValid(request.Currency);
            bool toCurrencyIsValid = await _createtransactionRepository.IsCurrencyValid(request.ToCurrency);

            if (!fromCurrencyIsValid || !toCurrencyIsValid)
            {
                throw new ArgumentException("One or both currency codes are invalid.");
            }
        }
    }
}