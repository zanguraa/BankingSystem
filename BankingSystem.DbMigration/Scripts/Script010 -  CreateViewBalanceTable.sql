USE BankingSystem_db
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BalanceViews]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BalanceViews](
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] NVARCHAR(450) NOT NULL,
    [ViewedAt] DATETIME NOT NULL,
    [Balance] DECIMAL(18, 2),
    [Currency] NVARCHAR(3),
)
END
GO