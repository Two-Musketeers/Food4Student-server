using System;

namespace API.DTOs;

public class VariationOptionCreateDto
{
    public string? Name { get; set; }
    public int PriceAdjustment { get; set; }
}
