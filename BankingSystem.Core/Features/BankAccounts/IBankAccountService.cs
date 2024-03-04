﻿using BankingSystem.Core.Features.BankAccounts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.BankAccounts
{
    public interface IBankAccountService
    {
        Task<List<int>> CreateBankAccount(CreateBankAccountRequest createBankAccountRequest);
        Task<bool> AddFunds(AddFundsRequest addFundsRequest);
		Task<bool> ValidateAccountAsync(int accountId);
        Task<bool> CheckAccountOwnershipAsync(int accountId, string userId);
    }
}
