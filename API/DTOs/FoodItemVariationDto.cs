namespace API.DTOs;

public class FoodItemVariationDto
{
    public string? FoodItemId { get; set; }
    public string? VariationId { get; set; }
    public string? VariationOptionId { get; set; }
    public string? VariationName { get; set; }
    public string? VariationOptionName { get; set; }
    public int PriceAdjustment { get; set; }
}