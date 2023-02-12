using Typro.Application.Repositories;

namespace Typro.Application.UnitsOfWork;

public interface IUnitOfWork
{
    public IUserRepository UserRepository { get; }
    public ITokenRepository TokenRepository { get; }
    public void BeginTransaction();
    public void RollbackTransaction();
    public void CommitTransaction();
}