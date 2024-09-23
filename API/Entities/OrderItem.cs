using System;

namespace API.Entities;

public class OrderItem
{
    // Composite Key
    public string? OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public string FoodName { get; set; } = null!;

    // Additional Properties
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageLink { get; set; } = null!;

    // Foreign Key to Original FoodItem
    public string? OriginalFoodItemId { get; set; }
    public FoodItem? OriginalFoodItem { get; set; }
}