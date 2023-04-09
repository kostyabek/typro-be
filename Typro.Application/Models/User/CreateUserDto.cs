using Typro.Domain.Enums.User;

namespace Typro.Application.Models.User;

public record CreateUserDto(
    string Email,
    string PasswordHash,
    UserRole RoleId,
    int TrainingConfigurationId,
    string Nickname,
    DateTime CreatedDate);