using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class OrderCreateDto
{
    [Required(ErrorMessage = "At least one order item is required.")]
    [MinLength(1, ErrorMessage = "At least one order item is required.")]
    public List<CreateOrderItemDto> OrderItems { get; set; } = [];
    [Required(ErrorMessage = "ShippingAddress is required.")]
    public required string ShippingAddress { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    [Required(ErrorMessage = "PhoneNumber is required.")]
    public required string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Name is required.")]
    public required string Name { get; set; }
    public string? Note { get; set; }

    [Required(ErrorMessage = "RestaurantId is required.")]
    public string? RestaurantId { get; set; }
}
