using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.Core.Features.BankAccounts.CreateBankAccount;
using Dapper;
using Microsoft.Data.SqlClient;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly string _connectionString;

    public BankAccountRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public async Task CreateBankAccountAsync(BankAccount bankAccount)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var commandText = @"
                INSERT INTO BankAccounts ( UserId, Iban, InitialAmount, Currency)
                VALUES (@UserId, @Iban, @InitialAmount, @Currency);";

            await connection.ExecuteAsync(commandText, new
            {
                bankAccount.UserId,
                bankAccount.Iban,
                bankAccount.InitialAmount,
                Currency = bankAccount.Currency.ToString()
            });
        }
    }

    public async Task<List<BankAccount>> GetBankAccounts()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = "SELECT Id, UserId, Iban, InitialAmount, Currency FROM BankAccounts";
            return (await connection.QueryAsync<BankAccount>(query)).ToList();
        }
    }
}
