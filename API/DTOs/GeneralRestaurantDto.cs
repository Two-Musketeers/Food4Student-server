using System;
using API.Entities;

namespace API.DTOs;

public class GeneralRestaurantDto
{
    public string? Id { get; set; }
    public bool IsApproved { get; set; }
    public string? Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Photo? Logo { get; set; }
    public int TotalRatings { get; set; }
    public double AverageRating { get; set; }
}
