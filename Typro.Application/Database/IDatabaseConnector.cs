using System.Data;

namespace Typro.Application.Database;

public interface IDatabaseConnector : IDisposable
{
    IDbConnection GetConnection();
}