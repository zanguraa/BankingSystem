namespace BankingSystem.Core.Data
{
    public interface IDataManager
    {
        Task<int> Execute<T>(string sql, T item);
        Task<IEnumerable<T>> Query<T, P>(string sql, P parameters);
        Task<IEnumerable<T>> Query<T>(string sql);
        Task<bool> ExecuteWithTransaction(List<SqlCommand> dataRequest);
    }
}
