using System;

namespace API.Entities;

public class FoodItemVariation
{
    public string? FoodItemId { get; set; }
    public string? VariationOptionId { get; set; }
    public string? VariationId { get; set; }
    public FoodItem FoodItem { get; set; } = null!;
    public VariationOption VariationOption { get; set; } = null!;
    public Variation Variation { get; set; } = null!;
}
