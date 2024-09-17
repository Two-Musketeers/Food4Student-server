namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public required string Address { get; set; }
    public required string Location { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string PhoneNumber { get; set; }
    public required Photo Avatar { get; set; }
    public List<RestaurantLike> FavoriteRestaurants { get; set; } = []; 
    public List<Rating> Ratings { get; set; } = [];
}