USE [BankingSystem_db]
GO

CREATE TABLE Transactions (
TransactionId BIGINT PRIMARY KEY IDENTITY(1,1),
FromAccountId INT NOT NULL,
ToAccountId INT NOT NULL,
FromAccountCurrency VARCHAR(10) NOT NULL,
ToAccountCurrency VARCHAR(10) NOT NULL,
FromAmount DECIMAL(18, 5) NOT NULL,
ToAmount DECIMAL(18, 5) NOT NULL,
Fee DECIMAL(18, 5),
TransactionDate DATETIME NOT NULL,
TransactionType INT NOT NULL,
FOREIGN KEY (FromAccountId) REFERENCES BankAccounts(Id),
FOREIGN KEY (ToAccountId) REFERENCES BankAccounts(Id)
);