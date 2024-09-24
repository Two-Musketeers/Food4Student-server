using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppUser : IdentityUser<string>
{
    public new string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Address { get; set; }
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
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