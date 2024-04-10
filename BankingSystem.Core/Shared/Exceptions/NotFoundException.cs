namespace BankingSystem.Core.Shared.Exceptions
{
	public class NotFoundException : DomainException
	{
		private static readonly int _statusCode = 404; 
		public NotFoundException(string message, params object?[]? parameters) : base(message, _statusCode, parameters) { }
	}
}
