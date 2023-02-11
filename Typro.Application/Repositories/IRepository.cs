using System.Data;

namespace Typro.Application.Repositories;

public interface IRepository
{
    IDbTransaction BeginTransaction();
}