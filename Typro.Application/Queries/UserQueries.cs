namespace Typro.Application.Queries;

public static class UserQueries
{
    public const string InsertUser = @"
INSERT INTO dbo.Users(Email, PasswordHash, RoleId, TrainingConfigurationId, CreatedDate, Nickname)
OUTPUT INSERTED.Id
VALUES (@Email, @PasswordHash, @RoleId, @TrainingConfigurationId, @CreatedDate, @Nickname);";

    public const string GetUserById = @"
SELECT
    Id,
    Email,
    PasswordHash,
    RoleId,
    TrainingConfigurationId,
    CreatedDate,
    Nickname
FROM dbo.Users
WHERE Id = @UserId;";

    public const string GetUserByEmail = @"
SELECT
    Id,
    Email,
    PasswordHash,
    RoleId,
    TrainingConfigurationId,
    CreatedDate,
    Nickname
FROM dbo.Users 
WHERE Email = @UserEmail;";
}