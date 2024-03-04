USE [BankingSystem_db]
GO

CREATE TABLE Deposits (
Id INT PRIMARY KEY IDENTITY(1,1),
BankAccountId INT,
Amount DECIMAL(18,5),
Date DATETIME
FOREIGN KEY (BankAccountId) REFERENCES BankAccounts(Id),
)