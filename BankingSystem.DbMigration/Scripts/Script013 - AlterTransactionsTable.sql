USE BankingSystem_db
GO

ALTER TABLE dbo.Transactions
ALTER COLUMN ToAccountId int NULL;

ALTER TABLE dbo.Transactions
ALTER COLUMN ToAmount decimal(18, 5) NULL;
