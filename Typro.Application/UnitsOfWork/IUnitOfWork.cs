using Typro.Application.Repositories;

namespace Typro.Application.UnitsOfWork;

public interface IUnitOfWork
{
    public IUserRepository UserRepository { get; }
    public ITokenRepository TokenRepository { get; }
    public ITrainingConfigurationRepository TrainingConfigurationRepository { get; }
    public ISupportedLanguagesRepository SupportedLanguagesRepository { get; }
    public IWordRepository WordRepository { get; } 
    public ITrainingResultsRepository TrainingResultsRepository { get; } 
    public void BeginTransaction();
    public void RollbackTransaction();
    public void CommitTransaction();
}