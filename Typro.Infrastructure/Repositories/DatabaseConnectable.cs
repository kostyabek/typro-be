using Typro.Application.Models.Database;

namespace Typro.Infrastructure.Repositories;

public abstract class DatabaseConnectable
{
    protected ConnectionWrapper ConnectionWrapper { get; }
    
    protected DatabaseConnectable(ConnectionWrapper connectionWrapper)
    {
        ConnectionWrapper = connectionWrapper;
    }
}