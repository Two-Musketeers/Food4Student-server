using API.Entities;

namespace API.DTOs;

public class RestaurantDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Photo? Logo { get; set; }
    public Photo? Banner { get; set; }
    public List<FoodItemDto>? Menu { get; set; }
    public List<RestaurantLike>? LikedByUsers { get; set; }
    public List<Rating>? Ratings { get; set; }
    public double AverageRating => Ratings?.Count != 0 ? Ratings.Average(r => r.Stars) : 0;
}
