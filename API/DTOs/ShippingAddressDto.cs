using System.Text.Json.Serialization;
using API.Entities;

namespace API.DTOs;

public class ShippingAddressDto
{
    public string? Id { get; set; }
    public string? Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Name { get; set; }
    public ShippingLocationType LocationType { get; set; }
    public string? Location { get; set; }
    public string? BuildingNote { get; set; }
    public string? OtherLocationTypeTitle { get; set; }
}