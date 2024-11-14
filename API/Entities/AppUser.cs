using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class AppUser
{
    [Key]
    public required string Id { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string PhoneNumber { get; set; }
    public Photo? Avatar { get; set; }
    public List<ShippingAddress> ShippingAddresses { get; set; } = [];
    public List<Order> Orders { get; set; } = [];
    public List<RestaurantLike> FavoriteRestaurants { get; set; } = [];
    public Restaurant? OwnedRestaurant { get; set; }
    public List<Rating> Ratings { get; set; } = [];
    //Role navigation
    public int AppRoleId { get; set; }
    public AppRole AppRole { get; set; } = null!;

}