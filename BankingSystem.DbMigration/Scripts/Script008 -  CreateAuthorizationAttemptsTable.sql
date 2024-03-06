USE BankingSystem_db
GO

    CREATE TABLE AuthorizationAttempts
(
    AttemptId INT IDENTITY(1,1) PRIMARY KEY,
    CardNumber NVARCHAR(16) NOT NULL,
    IsSuccess BIT NOT NULL,
    AttemptDate DATETIME NOT NULL
);