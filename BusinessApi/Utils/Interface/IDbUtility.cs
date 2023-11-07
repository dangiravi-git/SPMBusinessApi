using System.Data;

namespace BusinessApi.Utils
{
    public interface IDbUtility
    {
        Task<DataTable> ExecuteQuery(string query);
        Task<int> QueryExec(string sql);
        Task<string> QN(string s);
        Task<string> QS(string s, bool allowEmptyString = false, bool includeEqual = false);
    }
}
