namespace API.Entities;

public class Order
{
    public int Id { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];

    //AppUser navigation purpose
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
}
