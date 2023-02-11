namespace Typro.Application.Models.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    public string SecretKey { get; set; }
    public int TokenLifetimeInMinutes { get; set; }
}