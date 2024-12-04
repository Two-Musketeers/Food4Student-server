namespace API.Entities;

public class FoodItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Photo? FoodItemPhoto { get; set; }
    public required int BasePrice { get; set; }
    public ICollection<FoodItemVariation> FoodItemVariations { get; set; } = [];
    //Navigation properties (required for entity framework one to many relationship)
    public string? FoodCategoryId { get; set; }
    public FoodCategory FoodCategory { get; set; } = null!;
}
