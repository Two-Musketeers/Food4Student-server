using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class VariationCreateDto
{
    public string? Name { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "MinSelect must be non-negative.")]
    public int MinSelect { get; set; }
    public int? MaxSelect { get; set; }
    public List<VariationOptionCreateDto> VariationOptions { get; set; } = [];
}
