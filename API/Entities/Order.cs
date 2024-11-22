namespace API.Entities;
// TODO: add shipping address
public class Order
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public List<OrderItem> OrderItems { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Rating? Rating { get; set; }
    public string Status { get; set; } = "Pending";

    //AppUser navigation purpose
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;

    //Restaurant navigation purpose
    public string RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}
