using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class VariationSelectionDto
{
    [Required(ErrorMessage = "VariationId is required.")]
    public string? VariationId { get; set; }

    [Required(ErrorMessage = "VariationOptionIds are required.")]
    [MinLength(1, ErrorMessage = "At least one VariationOptionId must be selected.")]
    public List<string> VariationOptionIds { get; set; } = [];
}
