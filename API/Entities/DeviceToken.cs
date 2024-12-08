namespace API.Entities;

public class DeviceToken
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public string AppUserId { get; set; } = null!;
    public AppUser AppUser { get; set; } = null!;
}
