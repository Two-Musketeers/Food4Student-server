using API.Entities;

namespace API.DTOs;

public class FoodItemDto
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Photo? FoodItemPhoto { get; set; }
    public required double Price { get; set; }
}
