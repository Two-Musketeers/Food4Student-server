namespace API.Entities;

public class Variation
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public int MinSelect { get; set; } = 0;
    public int MaxSelect { get; set; } = 1;
    public ICollection<VariationOption> VariationOptions { get; set; } = [];
    public ICollection<FoodItemVariation> FoodItemVariations { get; set; } = [];
    //Navigation Properties
    public string? RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}