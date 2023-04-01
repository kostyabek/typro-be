using Typro.Domain.Enums.User;

namespace Typro.Domain.Database.Models;

public class User : BaseIdEntity
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public UserRole RoleId { get; set; }
    public int TrainingConfigurationId { get; set; }
    public TrainingConfiguration TrainingConfiguration { get; set; }
}