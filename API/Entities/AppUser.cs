using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class AppUser
{
    [Key]
    public required string Id { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Photo Avatar { get; set; }
    public List<Order> Orders { get; set; } = [];
    public List<RestaurantLike> FavoriteRestaurants { get; set; } = []; 
    public Restaurant? OwnedRestaurants { get; set; }
    public List<Rating> Ratings { get; set; } = [];
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
}