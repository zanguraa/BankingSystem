using BankingSystem.Core.Features.BankAccounts.CreateBankAccount;
using BankingSystem.Core.Features.BankAccounts.Requests;
using System;
using System.Threading.Tasks;

public interface IBankAccountRepository
{
    Task<BankAccount?> GetAccountByIbanAsync(string iban);
    Task<int> CreateBankAccountAsync(BankAccount bankAccount);
    Task<List<BankAccount>> GetBankAccounts();
    Task<bool> AddFunds(AddFundsRequest addFundsRequest);
    Task<bool> ExistsWithCurrencyAsync(int userId, string currency);
    }