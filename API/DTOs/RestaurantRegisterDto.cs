namespace API.DTOs;

public class RestaurantRegisterDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}