using System;

namespace API.DTOs;

public class RegisterRestaurantAccountDto
{
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public string? Description { get; set; }
    public required string OwnerPhoneNumber { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
