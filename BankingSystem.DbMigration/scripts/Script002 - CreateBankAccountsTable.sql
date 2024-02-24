USE [BankingSystem_db]
GO

CREATE TABLE [dbo].[BankAccounts](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [UserId] [int] NOT NULL FOREIGN KEY REFERENCES [dbo].[Users]([Id]),
    [Iban] [nvarchar](50) NOT NULL,
    [InitialAmount] [decimal](18, 2) NOT NULL,
    [Currency] [nvarchar](3) NOT NULL,
    CONSTRAINT [CK_Currency] CHECK ([Currency] IN ('GEL', 'USD', 'EUR')),
    CONSTRAINT [CK_InitialAmount] CHECK ([InitialAmount] >= 0)
)
GO