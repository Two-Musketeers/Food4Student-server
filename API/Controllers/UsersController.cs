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

public class UsersController(IShippingAddressRepository shippingAddressRepository,
    IUserRepository userRepository, IMapper mapper, ILikeRepository likeRepository,
    IRestaurantRepository restaurantRepository, IRatingRepository ratingRepository,
    IOrderRepository orderRepository, IUserNotificationRepository userNotificationRepository) : BaseApiController
{
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
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
    [Authorize(Policy = "RequireUserRole")]
    [HttpPost("shipping-addresses")]
    public async Task<ActionResult<ShippingAddressDto>> AddShippingAddress(CreateShippingAddressDto shippingAddressDto)
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

    [Authorize(Policy = "RequireUserRole")]
    [HttpPut("shipping-addresses/{addressId}")]
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

    [Authorize(Policy = "RequireUserRole")]
    [HttpGet("shipping-addresses")]
    public async Task<ActionResult<IEnumerable<ShippingAddressDto>>> GetShippingAddresses()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        var shippingAddresses = user.ShippingAddresses;

        return Ok(mapper.Map<IEnumerable<ShippingAddressDto>>(shippingAddresses));
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpGet("shipping-addresses/{shippingAddressId}")]
    public async Task<ActionResult<ShippingAddressDto>> GetShippingAddress(string shippingAddressId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var shippingAddress = await shippingAddressRepository.GetShippingAddressByIdAsync(shippingAddressId);

        if (shippingAddress == null) return NotFound("Shipping address not found");

        if (shippingAddress.AppUserId != userId) return Forbid();

        return Ok(mapper.Map<ShippingAddressDto>(shippingAddress));
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpDelete("shipping-addresses/{shippingAddressId}")]
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

    [Authorize(Policy = "RequireUserRole")]
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

    [Authorize(Policy = "RequireUserRole")]
    [HttpPost("likes/{restaurantId}")]
    public async Task<ActionResult> ToggleRestaurantLike(string restaurantId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await likeRepository.ToggleRestaurantLikeAsync(userId, restaurantId);

        if (result) return Ok();

        return BadRequest("Failed to like/unlike the restaurant");
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpPost("ratings/{orderId}")]
    public async Task<ActionResult> RateRestaurant(string orderId, CreatingRatingDto creatingRatingDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var order = await orderRepository.GetOrderByIdAsync(orderId);

        if (order == null) return NotFound("Order not found.");
        if (order.AppUserId != userId) return Unauthorized("This order does not belong to you.");
        var user = await userRepository.GetUserByIdAsync(userId);
        if (order.Status != "Delivered") return BadRequest("You can only rate a restaurant after the order has been delivered.");
        if (order.RestaurantId == null) return BadRequest("This order does not have a restaurant.");
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(order.RestaurantId);
        if (restaurant == null) return NotFound("Restaurant not found.");
        if (restaurant.IsApproved == false) return BadRequest("You cannot rate an unapproved restaurant.");
        if (order.Rating != null) return BadRequest("You have already rated this order.");

        var rating = new Rating
        {
            Id = order.Id,
            Stars = creatingRatingDto.Stars,
            Comment = creatingRatingDto.Comment,
            UserId = userId,
            RestaurantId = restaurant.Id
        };

        user.Ratings.Add(rating);
        restaurant.Ratings.Add(rating);
        ratingRepository.AddRating(rating);

        var result = await ratingRepository.SaveAllAsync();
        if (result) return CreatedAtAction(nameof(GetRatingByOrderId), new { orderId = order.Id }, mapper.Map<RatingDto>(rating));

        return BadRequest("Failed to rate the restaurant.");
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpGet("ratings")]
    public async Task<ActionResult> GetUserRatingsAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var ratings = await ratingRepository.GetUserRatingsAsync(userId);

        var ratingsDto = mapper.Map<IEnumerable<RatingDto>>(ratings);

        return Ok(ratingsDto);
    }

    [Authorize(Policy = "RequireUserRole")]
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

    [Authorize(Policy = "RequireUserRole")]
    [HttpGet("orders/{id}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var order = await orderRepository.GetOrderByIdAsync(id);
        var orderDto = mapper.Map<OrderDto>(order);
        if (order == null)
            return NotFound("Order not found.");

        if (order.AppUserId != userId)
            return Unauthorized("You do not have access to this order.");

        return Ok(orderDto);
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpGet("orders")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        IEnumerable<Order> orders;
        IEnumerable<OrderDto> ordersDto;

        orders = await orderRepository.GetOrdersByUserIdAsync(userId);
        ordersDto = mapper.Map<IEnumerable<OrderDto>>(orders);
        return Ok(ordersDto);
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpDelete("orders/{id}")]
    public async Task<ActionResult> DeleteOrder(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var order = await orderRepository.GetOrderByIdAsync(id);

        if (order == null)
            return NotFound("Order not found.");

        if (order.AppUserId != userId)
            return Unauthorized("You do not have access to delete this order.");

        if (order.Status != "Pending" || order.Status != "Delivered")
            return BadRequest("You cannot delete an order that is not pending or not delivered.");

        orderRepository.DeleteOrder(order);

        if (await orderRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to delete order.");
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpGet("notifications")]
    public async Task<ActionResult<IEnumerable<UserNotificationDto>>> GetNotifications()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var notifications = await userNotificationRepository.GetUserNotificationsAsync(userId);

        var notificationsDto = notifications.Select(n => new UserNotificationDto
        {
            Id = n.Id,
            Image = n.Image,
            Title = n.Title,
            Content = n.Content,
            Timestamp = n.Timestamp.ToUniversalTime(), // Ensures UTC
            IsUnread = n.IsUnread
        }).ToList();

        return Ok(notificationsDto);
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpPost("notifications")]
    public async Task<ActionResult<IEnumerable<UserNotificationDto>>> ReadAllNotifications()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var notifications = await userNotificationRepository.GetUserNotificationsAsync(userId);

        foreach (var notification in notifications)
        {
            notification.IsUnread = false;
        }

        var result = await userNotificationRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to mark notifications as read");

        return Ok(mapper.Map<IEnumerable<UserNotificationDto>>(notifications));
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpPost("notifications/{notificationId}")]
    public async Task<ActionResult<UserNotificationDto>> ReadNotification(string notificationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var notification = await userNotificationRepository.GetUserNotificationByIdAsync(notificationId);

        if (notification == null) return NotFound("Notification not found");

        if (notification.AppUserId != userId) return Forbid();

        notification.IsUnread = false;

        var result = await userNotificationRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to mark notification as read");

        return Ok(mapper.Map<UserNotificationDto>(notification));
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpDelete("notifications/{notificationId}")]
    public async Task<ActionResult> DeleteNotification(string notificationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var notification = await userNotificationRepository.GetUserNotificationByIdAsync(notificationId);

        if (notification == null) return NotFound("Notification not found");

        if (notification.AppUserId != userId) return Forbid();

        userNotificationRepository.RemoveUserNotification(notification);

        var result = await userNotificationRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to delete notification");

        return Ok("Notification has been deleted successfully");
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpDelete("notifications")]
    public async Task<ActionResult> DeleteAllNotifications()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var notifications = await userNotificationRepository.GetUserNotificationsAsync(userId);

        foreach (var notification in notifications)
        {
            userNotificationRepository.RemoveUserNotification(notification);
        }

        var result = await userNotificationRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to delete notifications");

        return Ok("Notifications have been deleted successfully");
    }
}