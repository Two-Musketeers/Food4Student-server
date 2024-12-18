using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class VariationCreateDto
{
    public string? Name { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "MinSelect must be non-negative.")]
    public int MinSelect { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "MaxSelect must be at least 1.")]
    public int MaxSelect { get; set; }
}
