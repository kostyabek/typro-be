namespace Typro.Domain.Database.Models;

public class BaseNamedEntity : BaseIdEntity
{
    public string Name { get; set; }
}