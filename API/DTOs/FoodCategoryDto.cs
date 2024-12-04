namespace API.DTOs;

public class FoodCategoryDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public IEnumerable<FoodItemDto>? FoodItems { get; set; }
}
