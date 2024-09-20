using FluentResults;
using Typro.Domain.Database.Models;

namespace Typro.Application.Services.Auth;

public interface ITokenService
{
    string GenerateAccessToken(Domain.Database.Models.User user);
    Task<RefreshToken> GenerateRefreshTokenAsync(int userId);
    Task<Result> ValidateRefreshToken(string token);
}