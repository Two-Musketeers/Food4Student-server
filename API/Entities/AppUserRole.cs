namespace API.Entities;

public class AppUserRole
{
    public string UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public string RoleId { get; set; }
    public AppRole Role { get; set; } = null!;
}
