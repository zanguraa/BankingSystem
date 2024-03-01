using BankingSystem.Core.Features.Transactions.CreateTransaction;
using BankingSystem.Core.Features.Transactions.TransactionService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

		[HttpPost("create")]
		public async Task<IActionResult> CreateTransaction(CreateTransactionRequest request)
		{
			try
			{
				var transactionResponse = await _transactionService.CreateTransactionAsync(request);
				return Ok(transactionResponse);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error");
			}
		}
	}
}
