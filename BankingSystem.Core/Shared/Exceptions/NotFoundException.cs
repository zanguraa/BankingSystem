namespace BankingSystem.Core.Shared.Exceptions
{
	public class NotFoundException : DomainException
	{
		public NotFoundException(string message, params object?[]? parameters) : base(message, parameters)
        {

		}
	}
}
