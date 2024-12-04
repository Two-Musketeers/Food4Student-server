using System;

namespace API.DTOs;

public class CreateOrderItemDto
{
    public string? FoodItemId { get; set; }
    public int Quantity { get; set; }
    public List<OrderItemVariationDto> SelectedVariations { get; set; } = [];
}
