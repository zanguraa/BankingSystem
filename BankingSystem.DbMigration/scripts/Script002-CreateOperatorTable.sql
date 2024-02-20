USE BankingSystem_db;

IF NOT EXISTS (SELECT * FROM [BankingSystem_db].INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Operator')
BEGIN
    CREATE TABLE Operator
    (
        OperatorId INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(50) NOT NULL,
        Password NVARCHAR(100) NOT NULL
    );
END;
ELSE
BEGIN
    PRINT 'The table Operator already exists.';
END;