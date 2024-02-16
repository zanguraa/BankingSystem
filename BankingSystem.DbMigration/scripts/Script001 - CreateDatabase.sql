IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'BankingSystem_db')
BEGIN
    CREATE DATABASE [BankingSystem_db];
END;