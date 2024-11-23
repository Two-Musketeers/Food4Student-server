namespace API.DTOs;

public class OrderCreateDto
{
    public List<CreateOrderItemDto> OrderItems { get; set; } = [];
    public required string ShippingInfoId { get; set; }
    public string? Note { get; set; }
}
