namespace Typro.Application.Models.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    public string SecretKey { get; init; }
    public int TokenLifetimeInMinutes { get; init; }
    public int RefreshTokenLifetimeInDays { get; init; }
}