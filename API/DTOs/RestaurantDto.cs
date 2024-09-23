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
    public int TotalRatings { get; set; }
    public double AverageRating { get; set; }
}
