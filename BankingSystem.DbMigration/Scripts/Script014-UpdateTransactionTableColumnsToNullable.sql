USE BankingSystem_db;
GO

ALTER TABLE dbo.Transactions
ALTER COLUMN FromAccountId int NULL;

ALTER TABLE dbo.Transactions
ALTER COLUMN FromAccountCurrency varchar(10) NULL;

ALTER TABLE dbo.Transactions
ALTER COLUMN ToAccountCurrency varchar(10) NULL;

ALTER TABLE dbo.Transactions
ALTER COLUMN FromAmount decimal(18, 5) NULL;
