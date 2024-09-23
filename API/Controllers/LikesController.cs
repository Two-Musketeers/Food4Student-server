using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController(ILikeRepository likeRepository, FirebaseAuthenticationService firebaseService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedList<RestaurantDto>>> GetRestaurantLikes([FromQuery] LikesParams likesParams)
    {
        var userId = await GetUserIdFromToken();
        if (userId == null) return Unauthorized();

        likesParams.UserId = userId;
        var restaurants = await likeRepository.GetRestaurantLikesAsync(likesParams);

        Response.AddPaginationHeader(restaurants.CurrentPage, restaurants.PageSize, restaurants.TotalCount, restaurants.TotalPages);

        return Ok(restaurants);
    }

    [HttpPost("{restaurantId}")]
    public async Task<ActionResult> ToggleRestaurantLike(string restaurantId)
    {
        var userId = await GetUserIdFromToken();
        if (userId == null) return Unauthorized();

        var result = await likeRepository.ToggleRestaurantLikeAsync(userId, restaurantId);

        if (result) return Ok();

        return BadRequest("Failed to like/unlike the restaurant");
    }

    private async Task<string?> GetUserIdFromToken()
    {
        var authHeader = Request.Headers.Authorization.FirstOrDefault();
        if (authHeader == null || !authHeader.StartsWith("Bearer ")) return null;

        var token = authHeader["Bearer ".Length..].Trim();
        return await firebaseService.VerifyIdToken(token);
    }
}
