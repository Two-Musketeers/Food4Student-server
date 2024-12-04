namespace API.Entities;

public class VariationOption
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int PriceAdjustment { get; set; }
    public required string Name { get; set; }
    public ICollection<FoodItemVariation> FoodItemVariations { get; set; } = [];
    //Navigation Properties
    public string? VariationId { get; set; }
    public Variation Variation { get; set; } = null!;
}
