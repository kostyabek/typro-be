using Typro.Domain.Database.Models;

namespace Typro.Application.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}