using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class OrderController(IMapper mapper,
    IOrderRepository orderRepository,
    IFoodItemRepository foodItemRepository) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(OrderCreateDto orderCreateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var order = new Order { AppUserId = userId };

        foreach (var itemDto in orderCreateDto.OrderItems)
        {
            var foodItem = await foodItemRepository.GetFoodItemByIdAsync(itemDto.FoodItemId);

            if (foodItem == null)
                return BadRequest($"FoodItem with ID '{itemDto.FoodItemId}' not found.");

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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
}
