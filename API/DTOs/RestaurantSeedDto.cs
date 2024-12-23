using System;
using API.Entities;

namespace API.DTOs;

public class RestaurantSeedDto
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Description { get; set; }
    public required string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Photo? Logo { get; set; }
    public Photo? Banner { get; set; }
    public bool IsApproved { get; set; } = false;
    public List<FoodCategory> FoodCategories { get; set; } = [];
}
