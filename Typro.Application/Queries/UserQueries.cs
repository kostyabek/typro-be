namespace Typro.Application.Queries;

public static class UserQueries
{
    public const string InsertUser = @"
INSERT INTO Users(Email, PasswordHash)
VALUES (@Email, @PasswordHash);";

    public const string GetUserById = @"
SELECT
    u.Email,
    u.PasswordHash,
FROM Users u
WHERE u.Id = @UserId;";

    public const string GetUserByEmail = @"
SELECT
    u.Email,
    u.PasswordHash
FROM Users u
WHERE u.Email = @UserEmail;";
}