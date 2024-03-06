namespace BankingSystem.Core.Data
{
    public class SqlCommandRequest
    {
        public string Query { get; set; }
        public object Params { get; set; } = new ();
    }
}