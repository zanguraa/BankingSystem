namespace BankingSystem.Core.Data
{
    public class SqlCommand
    {
        public string Query { get; set; }
        public object Params { get; set; } = new ();
    }
}