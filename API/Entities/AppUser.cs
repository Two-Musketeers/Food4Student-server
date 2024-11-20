using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class AppUser
{
    [Key]
    public required string Id { get; set; }
    public required string PhoneNumber { get; set; }
    public List<ShippingAddress> ShippingAddresses { get; set; } = [];
    public List<Order> Orders { get; set; } = [];
    public List<RestaurantLike> FavoriteRestaurants { get; set; } = [];
    public Restaurant? OwnedRestaurant { get; set; }
    public List<Rating> Ratings { get; set; } = [];
}