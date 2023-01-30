using System.Security.Cryptography;
using System.Text;
using Typro.Application.Services;

namespace Typro.Infrastructure.Services;

public class AuthService : IAuthService
{
    public void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var algo = new HMACSHA512();
        passwordSalt = algo.Key;
        passwordHash = algo.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
    {
        var algo = new HMACSHA512(Encoding.UTF8.GetBytes(passwordSalt));
        var providedPasswordHash = algo.ComputeHash(Encoding.UTF8.GetBytes(password));
        return providedPasswordHash.SequenceEqual(Encoding.UTF8.GetBytes(passwordHash));
    }
}