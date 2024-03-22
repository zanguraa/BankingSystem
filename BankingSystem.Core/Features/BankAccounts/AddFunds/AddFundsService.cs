using BankingSystem.Core.Shared.Exceptions;

namespace BankingSystem.Core.Features.BankAccounts.AddFunds
{
    public interface IAddFundsService
    {

    }

    public class AddFundsService : IAddFundsService
    {

        public async Task<bool> AddFunds(AddFundsRequest addFundsRequest)
        {
            ValidateAddFundsRequest(addFundsRequest);

            if (addFundsRequest == null || addFundsRequest.Amount == default || addFundsRequest.BankAccountId <= 0)
            {
                throw new Exception("Invalid request");
            }
            return await _bankAccountRepository.AddFunds(addFundsRequest);

        }

        private void ValidateAddFundsRequest(AddFundsRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "The request cannot be null.");
            }

            if (request.Amount <= 0)
            {
                throw new InvalidAddFundsValidatinException("The amount must be greater than zero.");
            }

            if (request.BankAccountId <= 0)
            {
                throw new InvalidAddFundsValidatinException("The Bank Account ID must be a positive number.");
            }

        }


    }
}
