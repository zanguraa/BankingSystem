using BankingSystem.Core.Features.BankAccounts.BankAccountsServices;
using BankingSystem.Core.Features.Transactions.CreateTransactions;
using BankingSystem.Core.Features.Transactions.TransactionServices;
using Microsoft.AspNetCore.Authorization;
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

		public TransactionController(ITransactionService transactionService, IBankAccountService bankAccountService)
		{
			_transactionService = transactionService;
			_bankAccountService = bankAccountService;
		}

		[HttpPost("transfer-transaction")]
		[Authorize("MyApiUserPolicy", AuthenticationSchemes = "Bearer")]
		public async Task<IActionResult> TransferTransaction([FromBody] CreateTransactionRequest request)
		{

			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			request.UserId = userId;

			var transactionResponse = await _transactionService.TransferTransactionAsync(request);
			return Ok(transactionResponse);
		}

		[HttpGet("get-transactions/{accountId}")]
		public async Task<IActionResult> GetTransactionsByAccountId(int accountId)
		{
			var transactions = await _transactionService.GetTransactionsByAccountIdAsync(accountId);
			return Ok(transactions);
		}
	}
}
