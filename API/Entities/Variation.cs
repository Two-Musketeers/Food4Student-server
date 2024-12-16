namespace API.Entities;

public class Variation
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public int MinSelect { get; set; } = 0;
    public int MaxSelect { get; set; } = 1;
    public ICollection<VariationOption> VariationOptions { get; set; } = [];
    //Navigation Properties
    public string? FoodItemId { get; set; }
    public FoodItem FoodItem { get; set; } = null!;
}