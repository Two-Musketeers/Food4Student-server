using API.DTOs;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class RatingController(IRatingRepository ratingRepository, FirebaseAuthenticationService firebaseService) : BaseApiController
{
    [HttpGet("average/{restaurantId}")]
    public async Task<ActionResult<double>> GetAverageRating(string restaurantId)
    {
        var averageRating = await ratingRepository.GetAverageRatingAsync(restaurantId);
        return Ok(averageRating);
    }

    [HttpPost("{restaurantId}")]
    public async Task<ActionResult> RateRestaurant(string restaurantId, [FromBody] RatingDto ratingDto)
    {
        var userId = await GetUserIdFromToken();
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        ratingDto.UserId = userId;
        ratingDto.RestaurantId = restaurantId;

        var result = await ratingRepository.AddOrUpdateRatingAsync(ratingDto);

        if (result) return Ok();

        return BadRequest("Failed to rate the restaurant");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateRating(int id, RatingDto ratingDto)
    {
        return await ratingRepository.UpdateRating(id, ratingDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRating(int id)
    {
        return await ratingRepository.DeleteRating(id);
    }
    private async Task<string?> GetUserIdFromToken()
    {
        var authHeader = Request.Headers.Authorization.FirstOrDefault();
        if (authHeader == null || !authHeader.StartsWith("Bearer ")) return null;

        var token = authHeader["Bearer ".Length..].Trim();
        return await firebaseService.VerifyIdToken(token);
    }
}

