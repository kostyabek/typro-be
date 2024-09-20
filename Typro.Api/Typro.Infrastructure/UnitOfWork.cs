using System.Data;
using Typro.Application.Models.Database;
using Typro.Application.Repositories;
using Typro.Application.UnitsOfWork;
using Typro.Infrastructure.Repositories.Auth;
using Typro.Infrastructure.Repositories.Training;
using Typro.Infrastructure.Repositories.User;

namespace Typro.Infrastructure;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    public IUserRepository UserRepository { get; }
    public ITokenRepository TokenRepository { get; }
    public ITrainingConfigurationRepository TrainingConfigurationRepository { get; }
    public ISupportedLanguagesRepository SupportedLanguagesRepository { get; }

    public IWordRepository WordRepository { get; }
    public ITrainingResultsRepository TrainingResultsRepository { get; }
    public IPreparedMultiplayerTextsRepository PreparedMultiplayerTextsRepository { get; }

    private readonly ConnectionWrapper _connectionWrapper;

    public UnitOfWork(IDbConnection connection)
    {
        _connectionWrapper = new ConnectionWrapper
        {
            Connection = connection,
            Transaction = null
        };
        
        UserRepository = new UserRepository(_connectionWrapper);
        TokenRepository = new TokenRepository(_connectionWrapper);
        TrainingConfigurationRepository = new TrainingConfigurationRepository(_connectionWrapper);
        SupportedLanguagesRepository = new SupportedLanguagesRepository(_connectionWrapper);
        WordRepository = new WordRepository(_connectionWrapper);
        TrainingResultsRepository = new TrainingResultsRepository(_connectionWrapper);
        PreparedMultiplayerTextsRepository = new PreparedMultiplayerTextsRepository(_connectionWrapper);
    }

    public void BeginTransaction()
    {
        if (_connectionWrapper.Connection.State != ConnectionState.Open)
        {
            _connectionWrapper.Connection.Open();
        }
        _connectionWrapper.Transaction = _connectionWrapper.Connection.BeginTransaction();
    }

    public void RollbackTransaction()
    {
        _connectionWrapper.Transaction?.Rollback();
        _connectionWrapper.Transaction?.Dispose();
    }

    public void CommitTransaction()
    {
        _connectionWrapper.Transaction?.Commit();
        _connectionWrapper.Transaction?.Dispose();
    }

    public void Dispose()
    {
        _connectionWrapper.Connection?.Dispose();
        _connectionWrapper.Transaction?.Dispose();
    }
}