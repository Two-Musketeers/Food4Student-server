using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class OrderCreateDto
{
    [Required(ErrorMessage = "At least one order item is required.")]
    [MinLength(1, ErrorMessage = "At least one order item is required.")]
    public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();

    [Required(ErrorMessage = "ShippingInfoId is required.")]
    public string? ShippingInfoId { get; set; }

    public string? Note { get; set; }

    [Required(ErrorMessage = "RestaurantId is required.")]
    public string? RestaurantId { get; set; }
}
