using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.DTOs;

public class RegisterDto
{
    [Required] public required string Id { get; set; }
    [Required] public required string Username { get; set; }
    [Required] public required string Email { get; set; }
    [Required] public required string PhoneNumber { get; set; }
    [Required] public required string Address { get; set; }
    [Required] public double Latitude { get; set; }
    [Required] public double Longitude { get; set; }
    [Required] public required Photo Avatar { get; set; }
}
