using System;
using API.Entities;

namespace API.DTOs;

public class LikeRestaurantDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Photo? Logo { get; set; }
    public List<Rating>? Ratings { get; set; }
    public double AverageRating => Ratings?.Count != 0 ? Ratings.Average(r => r.Stars) : 0;
}
