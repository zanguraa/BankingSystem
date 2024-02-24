using BankingSystem.Core.Features.BankAccounts.CreateBankAccount;
using System;
using System.Threading.Tasks;

public interface IBankAccountRepository
{
    Task CreateBankAccountAsync(BankAccount bankAccount);
    Task<List<BankAccount>> GetBankAccounts();
}