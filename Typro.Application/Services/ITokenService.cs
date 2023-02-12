using FluentResults;
using Typro.Domain.Database.Models;

namespace Typro.Application.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    Task<RefreshToken> GenerateRefreshTokenAsync(int userId);
    Task<Result> ValidateRefreshToken(string token);
}