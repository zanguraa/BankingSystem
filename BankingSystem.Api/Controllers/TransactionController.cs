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
        private readonly IBankAccountService _bankAccountService;

		public TransactionController(ITransactionService transactionService, IBankAccountService bankAccountService )
		{
			_transactionService = transactionService;
            _bankAccountService = bankAccountService;
		}

        [HttpPost("transfer-transaction")]
        [Authorize("MyApiUserPolicy", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> TransferTransaction([FromBody] CreateTransactionRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var ownsAccount = await _bankAccountService.CheckAccountOwnershipAsync(request.FromAccountId, userId);
                if (!ownsAccount)
                {
                    // Correctly signal a 403 Forbidden with a custom message
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "You do not have permission to access this account." });
                }

                var transactionResponse = await _transactionService.TransferTransactionAsync(request);
                return Ok(transactionResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
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
