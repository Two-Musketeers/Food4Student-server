namespace API.Entities;

public class Variation
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public bool IsMultiSelect { get; set; }
    public ICollection<VariationOption> VariationOptions { get; set; } = [];
    public ICollection<FoodItemVariation> FoodItemVariations { get; set; } = [];
    //Navigation Properties
    public string? RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}