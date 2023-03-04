namespace Typro.Application.Models.Options;

public class TokenOptions
{
    public const string SectionName = "Tokens";
    public string SecretKey { get; init; }
    public int TokenLifetimeInMinutes { get; init; }
    public int RefreshTokenLifetimeInDays { get; init; }
}