using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class AppRole
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
}
