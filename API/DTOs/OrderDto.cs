using System;

namespace API.DTOs;

public class OrderDto
{
    public string? Id { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = [];
    public OrderShippingAddressDto? ShippingAddress { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Note { get; set; }
    public string? Status { get; set; }
}
