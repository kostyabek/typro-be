using Typro.Application.Database;

namespace Typro.Infrastructure.Database;

public abstract class DatabaseConnectable
{
    protected readonly IDatabaseConnector DatabaseConnector;

    protected DatabaseConnectable(IDatabaseConnector databaseConnector)
        => DatabaseConnector = databaseConnector;
}