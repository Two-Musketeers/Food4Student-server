namespace API.Entities;

public class Restaurant
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required Photo Logo { get; set; }
    public required Photo Banner { get; set; }
    public required List<FoodItem> Menu { get; set; } = [];
    //public required StarRating Rating { get; set; }

}
