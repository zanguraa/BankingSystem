USE BankingSystem_db
GO

    CREATE TABLE ChangePinAttempts
    (
        ChangeId INT IDENTITY(1,1) PRIMARY KEY,
        CardNumber NVARCHAR(16) NOT NULL,
        ChangeTime DATETIME NOT NULL DEFAULT GETDATE(),
        WasSuccessful BIT NOT NULL,
        CONSTRAINT FK_ChangePinAttempts_Cards FOREIGN KEY (CardNumber) REFERENCES Cards(CardNumber)
    );