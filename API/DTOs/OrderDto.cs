using API.Entities;

namespace API.DTOs;

public class OrderDto
{
    public string? Id { get; set; }
    public string? RestaurantName { get; set; }
    public string? RestaurantId { get; set; }
    public string? PhotoUrl { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = [];
    public required string ShippingAddress { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TotalPrice { get; set; }
    public string? Note { get; set; }
    public OrderStatus Status { get; set; }
}