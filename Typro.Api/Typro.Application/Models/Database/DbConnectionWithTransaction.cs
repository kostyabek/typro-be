using System.Data;

namespace Typro.Application.Models.Database;

public class ConnectionWrapper
{
    public IDbConnection Connection { get; init; }
    public IDbTransaction? Transaction { get; set; }
}