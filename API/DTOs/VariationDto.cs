using System;

namespace API.DTOs;

public class VariationDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public IEnumerable<VariationOptionDto> VariationOptions { get; set; }
}
