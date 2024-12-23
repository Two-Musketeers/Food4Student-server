using System;

namespace API.DTOs;

public class FoodItemUpdateDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int BasePrice { get; set; }
    public List<VariationUpdateDto> Variations { get; set; } = [];
}
