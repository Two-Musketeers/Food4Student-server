namespace API.Entities;

public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string PhoneNumber { get; set; }
    public required Photo Avatar { get; set; }
    public List<Order> Orders { get; set; } = [];
    public List<RestaurantLike> FavoriteRestaurants { get; set; } = []; 
    public List<Restaurant> OwnedRestaurants { get; set; } = [];
    public List<Rating> Ratings { get; set; } = [];
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
}