namespace API.DTOs;

public class VariationOptionUpdateDto
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public int PriceAdjustment { get; set; }
}
