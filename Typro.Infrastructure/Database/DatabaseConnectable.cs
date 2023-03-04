using Typro.Application.Models.Database;

namespace Typro.Infrastructure.Database;

public abstract class DatabaseConnectable
{
    protected ConnectionWrapper ConnectionWrapper { get; }
    
    protected DatabaseConnectable(ConnectionWrapper connectionWrapper)
    {
        ConnectionWrapper = connectionWrapper;
    }
}