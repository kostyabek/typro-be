namespace Typro.Application.Models.User;

public record CreateUserModel
{
    public string Email { get; init; }
    public string PasswordHash { get; init; }
    public string PasswordSalt { get; init; }
}