namespace API.DTOs;

public class FoodItemDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? FoodItemPhotoUrl { get; set; }
    public int BasePrice { get; set; }
    public IEnumerable<FoodItemVariationDto>? FoodItemVariations { get; set; }
}