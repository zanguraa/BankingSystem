using BankingSystem.Core.Features.Transactions.CreateTransactions;
using BankingSystem.Core.Features.Transactions.InternalTransaction;
using BankingSystem.Core.Features.Transactions.Shared;
using BankingSystem.Core.Features.Transactions.Shared.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BankingSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ICreateTransactionService _transactionService;
    private readonly IInternalTransactionService _internalTransactionService;
    private readonly IExternalTransactionService _externalTransactionService;

    public TransactionController(ICreateTransactionService transactionService, IInternalTransactionService internalTransactionService, IExternalTransactionService externalTransactionService)
    {
        _transactionService = transactionService;
        _internalTransactionService = internalTransactionService;
        _externalTransactionService = externalTransactionService;

    }

    [HttpPost("internal")]
    [Authorize("MyApiUserPolicy", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> InternalTransaction([FromBody] CreateTransactionRequest request)
    {
        var transactionResponse = await _internalTransactionService.ProcessInternalTransactionAsync(request);
        return Ok(transactionResponse);
    }

    [HttpPost("external")]
    [Authorize("MyApiUserPolicy", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> ExternalTransaction([FromBody] CreateTransactionRequest request)
    {
        var transactionResponse = await _externalTransactionService.ProcessExternalTransactionAsync(request);
        return Ok(transactionResponse);
    }
}
