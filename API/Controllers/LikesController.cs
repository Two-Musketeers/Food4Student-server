using System.Security.Claims;
using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class LikesController(ILikeRepository likeRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedList<RestaurantDto>>> GetRestaurantLikes([FromQuery] LikesParams likesParams)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        likesParams.UserId = userId;
        var restaurants = await likeRepository.GetRestaurantLikesAsync(likesParams);

        Response.AddPaginationHeader(restaurants.CurrentPage, restaurants.PageSize, restaurants.TotalCount, restaurants.TotalPages);

        return Ok(restaurants);
    }

    [HttpPost("{restaurantId}")]
    public async Task<ActionResult> ToggleRestaurantLike(string restaurantId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await likeRepository.ToggleRestaurantLikeAsync(userId, restaurantId);

        if (result) return Ok();

        return BadRequest("Failed to like/unlike the restaurant");
    }
}
