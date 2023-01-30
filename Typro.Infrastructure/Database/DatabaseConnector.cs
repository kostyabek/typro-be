using System.Data;
using System.Data.SqlClient;
using Typro.Application.Database;

namespace Typro.Infrastructure.Database;

public class DatabaseConnector : IDatabaseConnector
{
    private readonly string _connectionString;
    private IDbConnection? _dbConnection;

    public DatabaseConnector(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return _dbConnection ??= new SqlConnection(_connectionString);
    }

    public void Dispose()
    {
        Console.WriteLine("=====IS DISPOSE=====");
        _dbConnection?.Close();
        _dbConnection?.Dispose();
    }
}