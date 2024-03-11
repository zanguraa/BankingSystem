USE BankingSystem_db
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DailyWithdrawals]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DailyWithdrawals](
        [DailyWithdrawalId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [BankAccountId] INT NOT NULL,
        [WithdrawalDate] DATE NOT NULL,
        [TotalAmount] DECIMAL(18,2) NOT NULL,
        [Currency] NVARCHAR(5) NOT NULL, /**chavamatet**/
        CONSTRAINT [FK_DailyWithdrawals_BankAccounts] FOREIGN KEY ([BankAccountId]) REFERENCES [dbo].[BankAccounts]([Id])
    );
    END