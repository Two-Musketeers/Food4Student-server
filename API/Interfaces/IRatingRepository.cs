using System;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces;

public interface IRatingRepository
{
    Task<double> GetAverageRatingAsync(string restaurantId);
    Task<bool> AddOrUpdateRatingAsync(RatingDto ratingDto);
    Task<ActionResult<RatingDto>> AddRating(RatingDto ratingDto);
    Task<ActionResult> UpdateRating(int id, RatingDto ratingDto);
    Task<ActionResult> DeleteRating(int id);
}
