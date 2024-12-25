using API.Entities;

namespace API.DTOs;

public class UserDto
{
    public string? Id { get; set; }
    public required string PhoneNumber { get; set; }
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public bool OwnedRestaurant { get; set; }
}
