using System;

namespace API.DTOs;

public class OrderShippingAddressDto
{
    public required string Address { get; set; }
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Name { get; set; }
}
