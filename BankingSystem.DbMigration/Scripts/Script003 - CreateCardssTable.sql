USE [BankingSystem_db]
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Cards' AND type = 'U')
BEGIN
    CREATE TABLE BankingSystem_db.dbo.Cards(
        Id INT PRIMARY KEY IDENTITY(1,1),
        CardNumber NVARCHAR(16) UNIQUE,
        FullName VARCHAR(100),
        ExpirationDate DATE,
        IsActive BIT, 
        Cvv INT,
        Pin INT,
        MaxTried INT,
        isLocked BIT,
        CreatedAt DATETIME,
        [UserId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Users]([Id]),
        [AccountId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[BankAccounts]([Id])
    );
END
GO
