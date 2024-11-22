using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "RequireUserRole")]
public class UsersController(IShippingAddressRepository shippingAddressRepository,
    IUserRepository userRepository, IMapper mapper, ILikeRepository likeRepository,
    IRestaurantRepository restaurantRepository, IRatingRepository ratingRepository,
    IOrderRepository orderRepository) : BaseApiController
{
    [HttpGet("owned-restaurant")]
    public async Task<ActionResult<RestaurantDto>> GetOwnedRestaurant()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return NotFound();

        var restaurantToReturn = mapper.Map<RestaurantDto>(user.OwnedRestaurant);

        return Ok(restaurantToReturn);
    }

    // Address Requests
    [HttpPost("addresses")]
    public async Task<ActionResult> AddShippingAddress(CreateShippingAddressDto shippingAddressDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        var shippingAddress = mapper.Map<ShippingAddress>(shippingAddressDto);

        user.ShippingAddresses.Add(shippingAddress);

        var result = await shippingAddressRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to add shipping address");

        return CreatedAtAction(nameof(GetShippingAddress), new { shippingAddressId = shippingAddress.Id }, mapper.Map<ShippingAddressDto>(shippingAddress));
    }

    [HttpPut("addresses/{addressId}")]
    public async Task<ActionResult> UpdateShippingAddress(string addressId, CreateShippingAddressDto shippingAddressDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var shippingAddress = await shippingAddressRepository.GetShippingAddressByIdAsync(addressId);
        if (shippingAddress == null) return NotFound("Shipping address not found");

        // Ensure the shipping address belongs to the authenticated user
        if (shippingAddress.AppUserId != userId) return Forbid();



        mapper.Map(shippingAddressDto, shippingAddress);

        var result = await shippingAddressRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to update shipping address");

        return Ok("Shipping address has been updated successfully");
    }

    [HttpGet("addresses")]
    public async Task<ActionResult<IEnumerable<ShippingAddressDto>>> GetShippingAddresses()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        var shippingAddresses = user.ShippingAddresses;

        return Ok(mapper.Map<IEnumerable<ShippingAddressDto>>(shippingAddresses));
    }

    [HttpGet("addresses/{shippingAddressId}")]
    public async Task<ActionResult<ShippingAddressDto>> GetShippingAddress(string shippingAddressId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var shippingAddress = await shippingAddressRepository.GetShippingAddressByIdAsync(shippingAddressId);

        if (shippingAddress == null) return NotFound("Shipping address not found");

        if (shippingAddress.AppUserId != userId) return Forbid();

        return Ok(mapper.Map<ShippingAddressDto>(shippingAddress));
    }

    [HttpDelete("addresses/{shippingAddressId}")]
    public async Task<ActionResult> DeleteShippingAddress(string shippingAddressId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var shippingAddress = await shippingAddressRepository.GetShippingAddressByIdAsync(shippingAddressId);

        if (shippingAddress == null) return NotFound("Shipping address not found");

        if (shippingAddress.AppUserId != userId) return Forbid();

        shippingAddressRepository.DeleteShippingAddress(shippingAddress);

        var result = await shippingAddressRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to delete shipping address");

        return Ok("Shipping address has been deleted successfully");
    }

    [HttpGet("likes")]
    public async Task<ActionResult<PagedList<RestaurantDto>>> GetRestaurantLikes([FromQuery] LikesParams likesParams)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        likesParams.UserId = userId;
        var restaurants = await likeRepository.GetRestaurantLikesAsync(likesParams);

        Response.AddPaginationHeader(restaurants.CurrentPage, restaurants.PageSize, restaurants.TotalCount, restaurants.TotalPages);

        return Ok(restaurants);
    }

    [HttpPost("likes/{restaurantId}")]
    public async Task<ActionResult> ToggleRestaurantLike(string restaurantId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await likeRepository.ToggleRestaurantLikeAsync(userId, restaurantId);

        if (result) return Ok();

        return BadRequest("Failed to like/unlike the restaurant");
    }

    [HttpPost("ratings/{orderId}")]
    public async Task<ActionResult> RateRestaurant(string orderId, CreatingRatingDto creatingRatingDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var order = await orderRepository.GetOrderByIdAsync(orderId);

            if (order == null) return NotFound("Order not found.");
            if (order.AppUserId != userId) return Unauthorized("This order does not belong to you.");
            var user = await userRepository.GetUserByIdAsync(userId);
            if (order.Status != "Delivered") return BadRequest("You can only rate a restaurant after the order has been delivered.");
            var restaurant = await restaurantRepository.GetRestaurantByIdAsync(order.RestaurantId);
            if (restaurant == null) return NotFound("Restaurant not found.");

            var ratingDto = mapper.Map<RatingDto>(creatingRatingDto);
            ratingDto.UserId = userId;
            ratingDto.RestaurantId = order.RestaurantId;
            ratingDto.OrderId = order.Id;
            var rating = mapper.Map<Rating>(ratingDto);

            user.Ratings.Add(rating);
            order.Rating = rating;
            ratingRepository.AddRating(rating);
            restaurant.Ratings.Add(rating);

            var result = await ratingRepository.SaveAllAsync();
            if (result) return CreatedAtAction(nameof(GetUserRatingsAsync), mapper.Map<RatingDto>(rating));

            return BadRequest("Failed to rate the restaurant.");
        }

    [HttpGet("ratings")]
    public async Task<ActionResult> GetUserRatingsAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var ratings = await ratingRepository.GetUserRatingsAsync(userId);

        var ratingsDto = mapper.Map<IEnumerable<RatingDto>>(ratings);

        return Ok(ratingsDto);
    } 

    [HttpGet("ratings/{orderId}")]
    public async Task<ActionResult> GetRatingByOrderId(string orderId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var rating = await ratingRepository.GetOrderRatingById(orderId);

        if (rating == null) return NotFound("Rating not found.");

        if (rating.UserId != userId) return Unauthorized("You do not have access to this rating.");

        var ratingDto = mapper.Map<RatingDto>(rating);

        return Ok(ratingDto);
    }
}