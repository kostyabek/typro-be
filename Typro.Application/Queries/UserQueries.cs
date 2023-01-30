namespace Typro.Application.Queries;

public static class UserQueries
{
    public const string InsertUser = @"
INSERT INTO Users(Email, PasswordHash, PasswordSalt)
VALUES (@Email, @PasswordHash, @PasswordSalt);";

    public const string GetUserById = @"
SELECT
    u.Email,
    u.PasswordHash,
    u.PasswordSalt
FROM Users
WHERE u.Id = @UserId;";

    public const string GetUserByEmail = @"
SELECT
    u.Email,
    u.PasswordHash,
    u.PasswordSalt
FROM Users
WHERE u.Email = @UserEmail;";
}