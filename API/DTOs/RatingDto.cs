namespace API.DTOs;

public class RatingDto
{
    public string? Id { get; set; }
    public int Stars { get; set; }
    public string? UserId { get; set; }
    public string? RestaurantId { get; set; }
    public string? Comment { get; set; }
    public string? OrderId { get; set; }
}
