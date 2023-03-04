namespace Typro.Application.Queries;

public static class UserQueries
{
    public const string CreateUser = @"
INSERT INTO dbo.Users(Email, PasswordHash, RoleId)
OUTPUT INSERTED.Id
VALUES (@Email, @PasswordHash, @RoleId);";

    public const string GetUserById = @"
SELECT
    Id,
    Email,
    PasswordHash,
    RoleId
FROM dbo.Users
WHERE Id = @UserId;";

    public const string GetUserByEmail = @"
SELECT
    Id,
    Email,
    PasswordHash,
    RoleId
FROM dbo.Users 
WHERE Email = @UserEmail;";
}