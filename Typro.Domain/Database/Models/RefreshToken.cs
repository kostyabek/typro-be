namespace Typro.Domain.Database.Models;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsRevoked { get; set; }
}