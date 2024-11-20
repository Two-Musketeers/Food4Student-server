using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class OrderController(IMapper mapper,
    IOrderRepository orderRepository,
    IFoodItemRepository foodItemRepository) : BaseApiController
{
    [Authorize(Policy = "RequireUserRole")]
    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(OrderCreateDto orderCreateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Ensure the order contains items
        if (orderCreateDto.OrderItems == null || !orderCreateDto.OrderItems.Any())
            return BadRequest("Order must contain at least one item.");

        // Get the restaurant ID from the first item
        var firstFoodItem = await foodItemRepository.GetFoodItemByIdAsync(orderCreateDto.OrderItems.First().FoodItemId);
        if (firstFoodItem == null)
            return BadRequest("Invalid FoodItem Id.");

        var restaurantId = firstFoodItem.RestaurantId;

        // Verify all items belong to the same restaurant
        foreach (var itemDto in orderCreateDto.OrderItems)
        {
            var foodItem = await foodItemRepository.GetFoodItemByIdAsync(itemDto.FoodItemId);
            if (foodItem == null)
                return BadRequest($"FoodItem with ID '{itemDto.FoodItemId}' not found.");

            if (foodItem.RestaurantId != restaurantId)
                return BadRequest("All items in the order must be from the same restaurant.");
        }

        var order = new Order
        {
            AppUserId = userId,
            RestaurantId = restaurantId
        };

        foreach (var itemDto in orderCreateDto.OrderItems)
        {
            var foodItem = await foodItemRepository.GetFoodItemByIdAsync(itemDto.FoodItemId);

            var orderItem = new OrderItem
            {
                OriginalFoodItemId = foodItem.Id,
                FoodName = foodItem.Name,
                FoodDescription = foodItem.Description,
                Price = (decimal)foodItem.Price,
                FoodItemPhoto = foodItem.FoodItemPhoto,
                Quantity = itemDto.Quantity
            };

            order.OrderItems.Add(orderItem);
        }

        orderRepository.AddOrder(order);

        if (await orderRepository.SaveAllAsync())
        {
            var orderDto = mapper.Map<OrderDto>(order);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, orderDto);
        }

        return BadRequest("Failed to create order.");
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var order = await orderRepository.GetOrderByIdAsync(id);

        if (order == null)
            return NotFound("Order not found.");

        if (order.AppUserId != userId)
            return Unauthorized("You do not have access to this order.");

        var orderDto = mapper.Map<OrderDto>(order);
        return Ok(orderDto);
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpGet("restaurant")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByRestaurant()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var orders = await orderRepository.GetOrdersByRestaurantIdAsync(userId);

        var ordersDto = mapper.Map<IEnumerable<OrderDto>>(orders);
        return Ok(ordersDto);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var orders = await orderRepository.GetOrdersByUserIdAsync(userId);

        var ordersDto = mapper.Map<IEnumerable<OrderDto>>(orders);
        return Ok(ordersDto);
    }

    
    [HttpDelete("{id}")]
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

    [HttpPut("{id}")]
    public async Task<ActionResult> ApproveOrder(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var order = await orderRepository.GetOrderByIdAsync(id);

        if (order == null)
            return NotFound("Order not found.");

        if (order.RestaurantId != userId)
            return Unauthorized("You do not have access to approve this order.");

        if (order.Status != "Pending")
            return BadRequest("You cannot approve an order that is not pending.");

        order.Status = "Approved";

        if (await orderRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to approve order.");
    }
}
