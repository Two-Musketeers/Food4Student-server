using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class Rating
{
    [Key]
    public string Id { get; set; }
    [Range(1, 5, ErrorMessage = "Stars must be between 1 and 5.")]
    public int Stars { get; set; }
    [MaxLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
    public string? Comment { get; set; }
    public string? UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public string? RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}
