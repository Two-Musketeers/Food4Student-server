namespace API.DTOs;

public class VariationDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public int MinSelect { get; set; }
    public int MaxSelect { get; set; }
    public IEnumerable<VariationOptionDto> VariationOptions { get; set; } = null!;
}