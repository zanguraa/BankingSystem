using Azure.Core;
using BankingSystem.Core.Features.Transactions;
using BankingSystem.Core.Features.Transactions.CreateTransactions;
using BankingSystem.Core.Features.Transactions.TransactionServices;
using BankingSystem.Core.Features.Transactions.TransactionsRepositories;
using BankingSystem.Core.Shared.Exceptions;
using System;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.BankAccounts.AddFunds
{
    public interface IAddFundsService
    {
        Task<bool> AddFunds(AddFundsRequest addFundsRequest);
    }

    public class AddFundsService : IAddFundsService
    {
        private readonly IAddFundsRepository _addFundsRepository;
        private readonly ITransactionRepository _transactionRepository; 

        public AddFundsService(IAddFundsRepository addFundsRepository, ITransactionRepository transactionRepository)
        {
            _addFundsRepository = addFundsRepository;
            _transactionRepository = transactionRepository; 
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

                return await _transactionRepository.ProcessDepositTransactionAsync(transaction);
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
                throw  new InvalidAddFundsValidationException("The Bank Account ID must be a positive number.");
            }
        }
    }
}
