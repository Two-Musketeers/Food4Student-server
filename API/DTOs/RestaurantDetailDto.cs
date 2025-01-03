namespace API.DTOs;

public class RestaurantDetailDto
{
    public required string Id { get; set; }
    public required bool IsApproved { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public int TotalRatings { get; set; }
    public double AverageRating { get; set; }
    public bool IsFavorited { get; set; }
    public IEnumerable<FoodCategoryDto>? FoodCategories { get; set; }
}
