namespace API.Entities;

public class AppRole
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<AppUser> Users { get; set; } = [];
}
