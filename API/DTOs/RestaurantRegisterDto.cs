using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RestaurantRegisterDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Address { get; set; }
    [Range(-90.0, 90.0, ErrorMessage = "Latitude must be between -90 and 90.")]
    public double Latitude { get; set; }

    [Range(-180.0, 180.0, ErrorMessage = "Longitude must be between -180 and 180.")]
    public double Longitude { get; set; }
}