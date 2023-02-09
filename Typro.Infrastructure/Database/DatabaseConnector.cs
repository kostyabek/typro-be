using System.Data;
using System.Data.SqlClient;
using Typro.Application.Database;

namespace Typro.Infrastructure.Database;

public class DatabaseConnector : IDatabaseConnector
{
    private readonly string _connectionString;
    private IDbConnection? _dbConnection;

    public DatabaseConnector(string connectionString)
        => _connectionString = connectionString;

    public IDbConnection GetConnection()
        => _dbConnection ??= new SqlConnection(_connectionString);

    public void Dispose()
        => _dbConnection?.Close();
}