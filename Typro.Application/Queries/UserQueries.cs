namespace Typro.Application.Queries;

public static class UserQueries
{
    public const string InsertUser = @"
INSERT INTO Users(Email, PasswordHash, RoleId)
VALUES (@Email, @PasswordHash, @RoleId);";

    public const string GetUserById = @"
SELECT
    u.Email,
    u.PasswordHash,
    u.RoleId
FROM Users u
WHERE u.Id = @UserId;";

    public const string GetUserByEmail = @"
SELECT
    u.Email,
    u.PasswordHash,
    u.RoleId
FROM Users u
WHERE u.Email = @UserEmail;";
}