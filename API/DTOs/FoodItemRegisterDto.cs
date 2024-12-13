namespace API.DTOs;

public class FoodItemRegisterDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public string? FoodCategoryId { get; set; }
}
