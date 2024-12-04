namespace API.Entities;

public class FoodCategory
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public ICollection<FoodItem> FoodItems { get; set; } = [];
    //Navigation Properties
    public string? RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}