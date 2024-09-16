using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("FoodItems")]
public class FoodItem
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required Photo FoodItemPhoto { get; set; }
    public required double Price { get; set; }

    //Navigation properties (required for entity framework one to many relationship)
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}
