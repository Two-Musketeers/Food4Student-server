using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace API.Entities;

public class Restaurant
{
    [MaxLength(30)]
    public string? Id { get; set; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Description { get; set; }
    public required string Address { get; set; }
    public Point Location { get; set; } = new Point(0, 0) { SRID = 4326 };
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