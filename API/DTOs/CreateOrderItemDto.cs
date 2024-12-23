using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreateOrderItemDto
{
    [Required(ErrorMessage = "FoodItemId is required.")]
    public string? FoodItemId { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "SelectedVariations is required.")]
    public List<VariationSelectionDto?> SelectedVariations { get; set; } = [];
}
