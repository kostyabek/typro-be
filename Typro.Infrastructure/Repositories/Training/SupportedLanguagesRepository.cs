using Dapper;
using Typro.Application.Models.Database;
using Typro.Application.Queries;
using Typro.Application.Repositories;
using Typro.Domain.Database.Models;

namespace Typro.Infrastructure.Repositories.Training;

public class SupportedLanguagesRepository(ConnectionWrapper connectionWrapper) : DatabaseConnectable(connectionWrapper), ISupportedLanguagesRepository
{
    public Task<IEnumerable<SupportedLanguage>> GetSupportedLanguagesAsync()
        => ConnectionWrapper.Connection.QueryAsync<SupportedLanguage>(SupportedLanguagesQueries.GetSupportedLanguages,
            transaction: ConnectionWrapper.Transaction);
}