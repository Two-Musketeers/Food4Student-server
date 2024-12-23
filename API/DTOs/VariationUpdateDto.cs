namespace API.DTOs;

public class VariationUpdateDto
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public int MinSelect { get; set; }
    public int? MaxSelect { get; set; }
    public List<VariationOptionUpdateDto> VariationOptions { get; set; } = [];
}