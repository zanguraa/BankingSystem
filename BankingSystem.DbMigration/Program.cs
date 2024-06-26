﻿using System.Reflection;
using Microsoft.Extensions.Configuration;
using DbUp;

public class Program
{
	static int Main(string[] args)
	{
		var config = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.Build();

		string username = Environment.UserName;

		var connectionString = config.GetConnectionString(username);
		EnsureDatabase.For.SqlDatabase(connectionString);

		var upgrader =
			DeployChanges.To
				.SqlDatabase(connectionString)
				.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
				.LogToConsole()
				.Build();

		var result = upgrader.PerformUpgrade();

		if (!result.Successful)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(result.Error);
			Console.ResetColor();
#if DEBUG
			Console.ReadLine();
#endif
			return -1;
		}

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine("Success!");
		Console.ResetColor();
		return 0;
	}
}