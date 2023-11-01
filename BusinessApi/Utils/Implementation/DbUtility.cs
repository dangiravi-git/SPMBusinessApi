using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace BusinessApi.Utils
{
    public class DbUtility : IDbUtility
    {
        private readonly string _connectionString;

        public DbUtility(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MyDatabase");
        }

        public async Task<DataTable> ExecuteQuery(string query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    var dataTable = new DataTable();
                    var dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }
        public async Task<int> QueryExec(string sql)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    return command.ExecuteNonQuery(); 
                }
            }
        }
        public async Task<string> QN(string s)
        {
            if (s != null)
            {
                s = Regex.Replace(s, @"\s", "");
                if (!decimal.TryParse(s, out decimal n))
                {
                    throw new InvalidOperationException("DbUtility.QN: Value must be numeric.");
                }
                if (n == decimal.MinValue)
                {
                    return "NULL";
                }
                return n.ToString().Replace(",", ".");
            }
            else
            {
                return "NULL";
            }
        }
    }
}
