using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Restaurants")]
public class Restaurant
{
    [Key]
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
    public required Photo Logo { get; set; }
    public required Photo Banner { get; set; }
    public required List<FoodItem> Menu { get; set; } = [];
    public List<RestaurantLike> LikedByUsers { get; set; } = [];
    public List<Rating> Ratings { get; set; } = [];
    public double AverageRating => Ratings.Count != 0 ? Ratings.Average(r => r.Stars) : 0;
}