namespace Typro.Application.Queries;

public static class RefreshTokenQueries
{
    public const string CreateToken = @"
INSERT INTO dbo.RefreshTokens(UserId, Token, CreatedDate, ExpirationDate, IsRevoked)
VALUES (@UserId, @Token, @CreatedDate, @ExpirationDate, @IsRevoked)";

    public const string GetRefreshTokenByToken = @"
SELECT
    Id,
    UserId,
    Token,
    CreatedDate,
    ExpirationDate,
    IsRevoked
FROM dbo.RefreshTokens
WHERE Token = @Token;";
}