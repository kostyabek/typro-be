using System.Data;

namespace Typro.Infrastructure.Database;

public abstract class DatabaseConnectable : IDisposable
{
    protected IDbConnection Connection { get; }
    protected IDbTransaction? Transaction { get; private set; }

    protected DatabaseConnectable(IDbConnection dbConnection)
        => Connection = dbConnection;

    public IDbTransaction BeginTransaction()
    {
        if (Connection.State == ConnectionState.Closed)
        {
            Connection.Open();
        }
        
        Transaction = Connection.BeginTransaction();
        return Transaction;
    }

    public void Dispose()
    {
        Transaction?.Dispose();
        Connection.Dispose();
    }
}