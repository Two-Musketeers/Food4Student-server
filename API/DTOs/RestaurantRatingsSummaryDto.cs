using System;

namespace API.DTOs;

public class RestaurantRatingsSummaryDto
{
    public int TotalRatings { get; set; }
    public double AverageRating { get; set; }
    public List<int>? PerStarRatings { get; set; }
    public List<RatingDto>? Ratings { get; set; }
}
