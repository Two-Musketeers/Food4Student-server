using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.DTOs;

public class CreateShippingAddressDto
{
    public string? Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Name { get; set; }
    public ShippingLocationType LocationType { get; set; }
    [Required(ErrorMessage = "Location is required")]
    public required string Location { get; set; }
    public string? BuildingNote { get; set; }
    public string? OtherLocationTypeTitle { get; set; }
}