using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.Core.Data;
using BankingSystem.Core.Features.BankAccounts.CreateBankAccount;
using Dapper;
using Microsoft.Data.SqlClient;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly IDataManager _dataManager;

    public BankAccountRepository(IDataManager dataManager)
    {
        _dataManager = dataManager;
    }

    public async Task<BankAccount?> GetAccountByIbanAsync(string iban)
    {
        string query = "SELECT * FROM BankAccounts WHERE Iban = @Iban";
        var result = await _dataManager.Query<BankAccount, dynamic>(query, new { iban });
        return result.FirstOrDefault();
    }

    public async Task<int> CreateBankAccountAsync(BankAccount bankAccount)
    {
        string query = @"
                INSERT INTO BankAccounts ( UserId, Iban, InitialAmount, Currency)
                VALUES (@UserId, @Iban, @InitialAmount, @Currency);";

        var result = await _dataManager.Execute(query, new
        {
            bankAccount.UserId,
            bankAccount.Iban,
            bankAccount.InitialAmount,
            Currency = bankAccount.Currency.ToString()
        });

        if (result == 0)
        {
            throw new Exception("Failed to create bank account");
        }

        var newBankAccount = await GetAccountByIbanAsync(bankAccount.Iban);
        if (newBankAccount == null)
        {
            throw new Exception("Failed to create bank account");
        }
        return newBankAccount.Id;

        //using (var connection = new SqlConnection())
        //{
        //    await connection.OpenAsync();

        //    var commandText = @"
        //        INSERT INTO BankAccounts ( UserId, Iban, InitialAmount, Currency)
        //        VALUES (@UserId, @Iban, @InitialAmount, @Currency);";

        //    await connection.ExecuteAsync(commandText, new
        //    {
        //        bankAccount.UserId,
        //        bankAccount.Iban,
        //        bankAccount.InitialAmount,
        //        Currency = bankAccount.Currency.ToString()
        //    });
        //}
    }

    public async Task<List<BankAccount>> GetBankAccounts()
    {
        using (var connection = new SqlConnection("string"))
        {
            await connection.OpenAsync();
            string query = "SELECT Id, UserId, Iban, InitialAmount, Currency FROM BankAccounts";
            return (await connection.QueryAsync<BankAccount>(query)).ToList();
        }
    }
}
