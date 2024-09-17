namespace API.Entities;

public class RestaurantLike
{
    public int SourceUserId { get; set; }
    public AppUser SourceUser { get; set; } = null!;
    public int LikedRestaurantId { get; set; }
    public Restaurant LikedRestaurant { get; set; } = null!;

}
