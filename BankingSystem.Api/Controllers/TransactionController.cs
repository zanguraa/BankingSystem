﻿using BankingSystem.Core.Features.Transactions.CreateTransactions;
using BankingSystem.Core.Features.Transactions.CreateTransactions.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankingSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ICreateTransactionService _transactionService;

    public TransactionController(ICreateTransactionService transactionService)
    {
        _transactionService = transactionService;

    }

    [HttpPost("internal")]
    [Authorize("MyApiUserPolicy", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> InternalTransaction([FromBody] CreateTransactionRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        request.UserId = userId;

        var transactionResponse = await _transactionService.ProcessInternalTransactionAsync(request);
        return Ok(transactionResponse);
    }

    [HttpPost("external")]
    [Authorize("MyApiUserPolicy", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> ExternalTransaction([FromBody] CreateTransactionRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        request.UserId = userId;

        var transactionResponse = await _transactionService.ProcessExternalTransactionAsync(request);
        return Ok(transactionResponse);
    }
}
