using BankingSystem.Core.Features.Transactions.CreateTransactions;
using BankingSystem.Core.Features.Transactions.TransactionsRepositories;
using BankingSystem.Core.Shared.Exceptions;

namespace BankingSystem.Core.Features.Transactions.TransactionServices
{
    public class TransactionServiceValidator : ITransactionServiceValidator
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionServiceValidator(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
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

            bool fromCurrencyIsValid = await _transactionRepository.IsCurrencyValid(request.Currency);
            bool toCurrencyIsValid = await _transactionRepository.IsCurrencyValid(request.ToCurrency);

            if (!fromCurrencyIsValid || !toCurrencyIsValid)
            {
                throw new ArgumentException("One or both currency codes are invalid.");
            }
        }
    }
}