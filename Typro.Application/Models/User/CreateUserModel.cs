using Typro.Domain.Enums;

namespace Typro.Application.Models.User;

public record CreateUserDto(string Email, string PasswordHash, UserRole RoleId);