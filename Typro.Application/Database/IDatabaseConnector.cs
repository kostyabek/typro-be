using System.Data;

namespace Typro.Application.Database;

public interface IDatabaseConnector
{
    IDbConnection CreateConnection();
}