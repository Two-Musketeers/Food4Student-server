namespace API.Entities;

public class ShippingAddress
{
    public int Id { get; set; }
    public required string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Name { get; set; }
    //Navigation
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
}
