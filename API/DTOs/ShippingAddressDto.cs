namespace API.DTOs;

public class ShippingAddressDto
{
    public string? Id { get; set; }
    public string? Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Name { get; set; }
}