USE [BankingSystem_db]
GO

CREATE TABLE [dbo].[BankAccounts](
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Users]([Id]),
    [Iban] NVARCHAR(50) NOT NULL,
    [InitialAmount] DECIMAL(18, 2) NOT NULL,
    [Currency] NVARCHAR(3) NOT NULL,
    CONSTRAINT [CK_Currency] CHECK ([Currency] IN ('GEL', 'USD', 'EUR')),
    CONSTRAINT [CK_InitialAmount] CHECK ([InitialAmount] >= 0)
)
GO