namespace API.Entities;

public class AppRole
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
}
