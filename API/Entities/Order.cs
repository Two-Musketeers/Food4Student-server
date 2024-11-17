namespace API.Entities;

public class Order
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public List<OrderItem> OrderItems { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Status { get; set; } = "Pending";

    //AppUser navigation purpose
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
}
