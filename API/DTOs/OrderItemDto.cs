using API.Entities;

namespace API.DTOs;

public class OrderItemDto
{
    public string? Id { get; set; }
    public string? FoodName { get; set; }
    public string? FoodDescription { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public PhotoDto? FoodItemPhoto { get; set; }
    public string? FoodItemId { get; set; }
}
