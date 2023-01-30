using System.Data;
using System.Data.SqlClient;
using Typro.Application.Database;

namespace Typro.Infrastructure.Database;

public class DatabaseConnector : IDatabaseConnector
{
    private readonly string _connectionString;

    public DatabaseConnector(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}