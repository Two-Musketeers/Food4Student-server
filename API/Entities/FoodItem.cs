using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("FoodItems")]
public class FoodItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Photo? FoodItemPhoto { get; set; }
    public required double Price { get; set; }

    //Navigation properties (required for entity framework one to many relationship)
    public string? RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}
