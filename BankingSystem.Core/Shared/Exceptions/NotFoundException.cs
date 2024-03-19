﻿namespace BankingSystem.Core.Shared.Exceptions
{
	public class NotFoundException : DomainException
	{
		public NotFoundException(string message) : base(message)
		{

		}
	}
}
