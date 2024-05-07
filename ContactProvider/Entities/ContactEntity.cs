using System.ComponentModel.DataAnnotations;

namespace ContactProvider.Entities;

public class ContactEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Service { get; set; }
    public string Message { get; set; } = null!;
}
