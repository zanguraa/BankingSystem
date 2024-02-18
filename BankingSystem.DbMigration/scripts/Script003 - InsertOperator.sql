USE BankingSystem_db;

IF NOT EXISTS (SELECT * FROM Operator)
BEGIN
    INSERT INTO Operator (UserName, Password)
    VALUES ('Operator', 'admin123');
END;
ELSE
BEGIN
    PRINT 'An operator already exists in the Operator table.';
END;