namespace API.Entities;

public class Rating
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int Stars { get; set; }
    public string? Comment { get; set; }
    public string? UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public string? RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}
