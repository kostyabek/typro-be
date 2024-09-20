using Typro.Application.Models.Database;

namespace Typro.Infrastructure.Repositories;

public abstract class DatabaseConnectable(ConnectionWrapper connectionWrapper)
{
    protected ConnectionWrapper ConnectionWrapper { get; } = connectionWrapper;
}