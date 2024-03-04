USE BankingSystem_db
GO

    INSERT INTO BankingSystem_db.dbo.Currencies(
	Name,
	Code,
	Rate
	) VALUES
	(
	'Georgian Lari',
	'GEL',
	'1'
	), 
	(
	'American Dollar',
	'USD',
	'2.6596'
	),
	(
	'Europian Euro',
	'EUR',
	'2.8745'
	)