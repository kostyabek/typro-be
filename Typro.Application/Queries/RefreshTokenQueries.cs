namespace Typro.Application.Queries;

public static class RefreshTokenQueries
{
    public const string CreateRefreshToken = @"
INSERT INTO RefreshTokens(UserId, Token, CreatedDate, ExpirationDate, IsRevoked)
VALUES (@UserId, @Token, @CreatedDate, @ExpirationDate, @IsRevoked)";
}