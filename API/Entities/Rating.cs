namespace API.Entities;

public class Rating
{
    public int Id { get; set; }
    public int Stars { get; set; }
    public string? UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public string? RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}
