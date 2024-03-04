using BankingSystem.Core.Features.BankAccounts;
using BankingSystem.Core.Features.Transactions.CreateTransactions;
using BankingSystem.Core.Features.Transactions.TransactionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankingSystem.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TransactionController : ControllerBase
	{
		private readonly ITransactionService _transactionService;

		public TransactionController(ITransactionService transactionService)
		{
			_transactionService = transactionService;
		}

		[HttpPost("create-transaction")]
        [Authorize(Policy = "MyApiUserPolicy")]
        public async Task<IActionResult> CreateInternalTransaction(CreateTransactionRequest request)
		{
			try
			{

                // Retrieve current user's ID from the claim
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Check if the AccountId belongs to the current user
                var ownsAccount = await _bankAccountService.CheckAccountOwnershipAsync(request.AccountId, userId);
                if (!ownsAccount)
                {
                    return Forbid("You do not have permission to access this account.");
                }

                var transactionResponse = await _transactionService.CreateTransactionAsync(request);
				return Ok(transactionResponse);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

        [HttpGet("get-transactions/{accountId}")]
        public async Task<IActionResult> GetTransactionsByAccountId(int accountId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsByAccountIdAsync(accountId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                // Log the exception for further investigation
                Console.WriteLine($"An error occurred while fetching transactions for account ID {accountId}: {ex}");

                // Return an appropriate error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
