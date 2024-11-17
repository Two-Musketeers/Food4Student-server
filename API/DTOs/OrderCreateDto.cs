namespace API.DTOs;

public class OrderCreateDto
{
    public List<OrderItemDto> OrderItems { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public string? Status { get; set; }
}
