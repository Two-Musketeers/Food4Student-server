using System;

namespace API.DTOs;

public class VariationCreateDto
{
    public string? Name { get; set; }
    public bool IsMultiSelect { get; set; }
}
