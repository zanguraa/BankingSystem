USE [BankingSystem_db]

ALTER TABLE [dbo].[Users]
ADD 
    [PersonalId] NVARCHAR(MAX) NULL,
    [BirthdayDate] DATETIME NULL;