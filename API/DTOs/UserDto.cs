using API.Entities;

namespace API.DTOs;

public class UserDto
{
    public string? Id { get; set; }
    public string? Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? PhoneNumber { get; set; }
    public Photo? Avatar { get; set; }
    public List<Order> Orders { get; set; } = [];
    public List<RestaurantLike>? FavoriteRestaurants { get; set; }
    public List<Restaurant>? OwnedRestaurants { get; set; }
    public ICollection<AppUserRole>? UserRoles { get; set; }
}
