namespace Typro.Application.Services;

public interface IAuthService
{
    void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt);
}