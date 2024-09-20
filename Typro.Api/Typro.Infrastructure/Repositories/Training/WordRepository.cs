using Dapper;
using Typro.Application.Models.Database;
using Typro.Application.Queries;
using Typro.Application.Repositories;
using Typro.Domain.Database.Models;

namespace Typro.Infrastructure.Repositories.Training;

public class WordRepository(ConnectionWrapper connectionWrapper) : DatabaseConnectable(connectionWrapper), IWordRepository
{
    public Task<IEnumerable<Word>> GetNRandomWordsByLanguageAsync(int languageId, int numberOfWords)
        => ConnectionWrapper.Connection.QueryAsync<Word>(WordsQueries.GetNRandomWordsByLanguage,
            new
            {
                NumberOfWords = numberOfWords,
                LanguageId = languageId
            },
            ConnectionWrapper.Transaction);
}