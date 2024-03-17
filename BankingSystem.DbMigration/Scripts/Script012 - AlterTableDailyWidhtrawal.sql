USE BankingSystem_db
GO

ALTER TABLE [BankingSystem_db].[dbo].[DailyWithdrawals]
ADD RequestedCurrency NVARCHAR(3),
    RequestedAmount DECIMAL(19,4);