using System.Data;
using Typro.Application.Models.Database;
using Typro.Application.Repositories;
using Typro.Application.UnitsOfWork;
using Typro.Infrastructure.Repositories;

namespace Typro.Infrastructure.UnitsOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    public IUserRepository UserRepository { get; }
    public ITokenRepository TokenRepository { get; }

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