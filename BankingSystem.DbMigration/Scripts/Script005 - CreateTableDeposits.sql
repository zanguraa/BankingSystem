USE [BankingSystem_db]
GO

IF OBJECT_ID('dbo.Deposits', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Deposits (
        Id INT PRIMARY KEY IDENTITY(1,1),
        BankAccountId INT,
        Amount DECIMAL(18,5),
        Date DATETIME,
        CONSTRAINT FK_Deposits_BankAccounts FOREIGN KEY (BankAccountId) REFERENCES BankAccounts(Id)
    )
END
GO