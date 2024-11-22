namespace API.Entities;

public class ShippingAddress
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Name { get; set; }
    //Navigation
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
}
