namespace API.Entities;

public class FoodItem
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required Photo FoodItemPhoto { get; set; }
    public required double Price { get; set; }

}
