using System;

namespace API.Entities;

public class OrderItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string FoodName { get; set; }
    public string? FoodDescription { get; set; }
    public required int Price { get; set; }
    public required int Quantity { get; set; }
    public Photo? FoodItemPhoto { get; set; }
    // Add this property to store selected variations and options
    public ICollection<OrderItemVariation> OrderItemVariations { get; set; } = [];
    // Order navigation purpose
    public string? OrderId { get; set; }
    public Order Order { get; set; } = null!;

    // Foreign Key to Original FoodItem
    public string? OriginalFoodItemId { get; set; }
    public FoodItem? OriginalFoodItem { get; set; }
}