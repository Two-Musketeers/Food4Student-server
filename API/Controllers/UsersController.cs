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
    IOrderRepository orderRepository, IFoodItemRepository foodItemRepository) : BaseApiController
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

    //Order controller
    [Authorize(Policy = "RequireUserRole")]
    [HttpPost("orders")]
    public async Task<ActionResult<OrderDto>> CreateOrder(OrderCreateDto orderCreateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var shippingAddress = await shippingAddressRepository.GetShippingAddressByIdAsync(orderCreateDto.ShippingInfoId);

        // Ensure the order contains items
        if (orderCreateDto.OrderItems == null || orderCreateDto.OrderItems.Count == 0)
            return BadRequest("Order must have at least one item.");
        if (orderCreateDto.OrderItems.Any(x => x.FoodItemId == null))
            return BadRequest("Invalid foodItem.");

        // Get the restaurant ID from the first item
        var firstFoodItem = await foodItemRepository.GetFoodItemByIdAsync(orderCreateDto.OrderItems.First().FoodItemId!);
        if (firstFoodItem == null)
            return BadRequest("FoodItem not found.");

        var restaurantId = firstFoodItem.RestaurantId;
        if (restaurantId == null)
            return BadRequest("Restaurant not found.");

        var order = new Order
        {
            AppUserId = userId,
            RestaurantId = restaurantId,
            ShippingAddress = shippingAddress,
        };
        // Verify all items belong to the same restaurant
        foreach (var itemDto in orderCreateDto.OrderItems)
        {
            var foodItem = await foodItemRepository.GetFoodItemByIdAsync(itemDto.FoodItemId!);
            if (foodItem == null)
                return BadRequest("FoodItem not found.");

            if (foodItem.RestaurantId != restaurantId)
                return BadRequest("All items in the order must be from the same restaurant.");
        }

        foreach (var itemDto in orderCreateDto.OrderItems)
        {
            var foodItem = await foodItemRepository.GetFoodItemByIdAsync(itemDto.FoodItemId!);

            var orderItem = new OrderItem
            {
                OriginalFoodItemId = foodItem!.Id,
                FoodName = foodItem.Name,
                FoodDescription = foodItem.Description,
                Price = foodItem.Price,
                FoodItemPhoto = foodItem.FoodItemPhoto,
                Quantity = itemDto.Quantity
            };

            order.OrderItems.Add(orderItem);
        }

        orderRepository.AddOrder(order);

        if (await orderRepository.SaveAllAsync())
        {
            var orderDto = mapper.Map<OrderDto>(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, orderDto);
        }

        return BadRequest("Failed to create order.");
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

        if (order.Status != "Pending")
            return BadRequest("You cannot delete an order that is not pending.");

        orderRepository.DeleteOrder(order);

        if (await orderRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to delete order.");
    }
}