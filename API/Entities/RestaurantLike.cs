namespace API.Entities;

public class RestaurantLike
{
    public string? SourceUserId { get; set; }
    public AppUser SourceUser { get; set; } = null!;
    public string? LikedRestaurantId { get; set; }
    public Restaurant LikedRestaurant { get; set; } = null!;

}
