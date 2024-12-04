using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class Restaurant
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Address { get; set; }
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
    public Photo? Logo { get; set; }
    public Photo? Banner { get; set; }
    public List<RestaurantLike> LikedByUsers { get; set; } = [];
    public List<FoodCategory> FoodCategories { get; set; } = [];
    public List<Variation> Variations { get; set; } = [];
    public List<Rating> Ratings { get; set; } = [];
    public List<Order> Orders { get; set; } = [];
    public bool IsApproved { get; set; } = false;
    public double AverageRating => Ratings.Count != 0 ? Ratings.Average(r => r.Stars) : 0;
}