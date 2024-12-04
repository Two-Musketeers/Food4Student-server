namespace API.DTOs;

public class OrderItemDto
{
    public string? Id { get; set;}
    public string? FoodName { get; set; }
    public string? FoodDescription { get; set; }
    public int Price { get; set; } // TODO: ID
    public int Quantity { get; set; }
    public string? FoodItemPhotoUrl { get; set; } // TODO: FoodItemPhotoUrl
    public List<OrderItemVariationDto> SelectedVariations { get; set; } = [];
}
