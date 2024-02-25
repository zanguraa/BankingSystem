using BankingSystem.Core.Features.BankAccounts.CreateBankAccount;
using System;
using System.Threading.Tasks;

public interface IBankAccountRepository
{
    Task<BankAccount?> GetAccountByIbanAsync(string iban);
    Task<int> CreateBankAccountAsync(BankAccount bankAccount);
    Task<List<BankAccount>> GetBankAccounts();
    }