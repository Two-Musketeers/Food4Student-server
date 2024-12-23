namespace API.DTOs;

public class OrderItemDto
{
    public string? Id { get; set;}
    public string? FoodName { get; set; }
    public string? FoodDescription { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public string? FoodItemPhotoUrl { get; set; }
    public string? OriginalFoodItemId { get; set; }
    public string? Variations { get; set; }
}