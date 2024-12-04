namespace API.Entities;

public class OrderItemVariation
{
    public string? OrderItemId { get; set; }
    public OrderItem OrderItem { get; set; } = null!;

    public string? VariationId { get; set; }
    public Variation Variation { get; set; } = null!;

    public string? VariationOptionId { get; set; }
    public VariationOption VariationOption { get; set; } = null!;
}
