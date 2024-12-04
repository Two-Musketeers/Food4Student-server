using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class OrdersController(IMapper mapper,
    IOrderRepository orderRepository,
    IShippingAddressRepository shippingAddressRepository,
    IVariationRepository variationRepository,
    IFoodItemRepository foodItemRepository,
    IVariationOptionRepository variationOptionRepository) : BaseApiController
{

    //Order controller
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpGet("orders")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var orders = await orderRepository.GetOrdersByRestaurantIdAsync(userId);

        var ordersDto = mapper.Map<IEnumerable<OrderDto>>(orders);
        return Ok(ordersDto);

    }

    [Authorize]
    [HttpGet("orders/{id}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(string id)
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
    [HttpPut("orders/{id}/approve-order")]
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
        if (order.Status == "Delivered")
            return BadRequest("You cannot approve an order that is already delivered.");

        order.Status = "Approved";

        if (await orderRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to approve order.");
    }

    [HttpPut("orders/{id}/reject-order")]
    public async Task<ActionResult> RejectOrder(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var order = await orderRepository.GetOrderByIdAsync(id);

        if (order == null)
            return NotFound("Order not found.");

        if (order.RestaurantId != userId)
            return Unauthorized("You do not have access to reject this order.");

        if (order.Status != "Pending")
            return BadRequest("You cannot reject an order that is not pending.");
        if (order.Status == "Delivered")
            return BadRequest("You cannot reject an order that is already delivered.");

        order.Status = "Rejected";

        if (await orderRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to reject order.");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPut("orders/{id}/deliver-order")]
    public async Task<ActionResult> DeliverOrder(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var order = await orderRepository.GetOrderByIdAsync(id);

        if (order == null)
            return NotFound("Order not found.");

        if (order.RestaurantId != userId)
            return Unauthorized("You do not have access to deliver this order.");
        if (order.Status != "Approved")
            return BadRequest("You cannot deliver an order that is not approved.");
        if (order.Status == "Delivered")
            return BadRequest("You cannot deliver an order that is already delivered.");
        if (order.Status == "Rejected")
            return BadRequest("You cannot deliver an order that is rejected.");

        order.Status = "Delivered";

        if (await orderRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to deliver order.");
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpPost("orders")]
    public async Task<ActionResult<OrderDto>> CreateOrder(OrderCreateDto orderCreateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var shippingAddress = await shippingAddressRepository.GetShippingAddressByIdAsync(orderCreateDto.ShippingInfoId);

        // Validate order items
        if (orderCreateDto.OrderItems == null || orderCreateDto.OrderItems.Count == 0)
            return BadRequest("Order must have at least one item.");

        // Get the restaurant ID from the first item
        var firstFoodItem = await foodItemRepository.GetFoodItemByIdAsync(orderCreateDto.OrderItems.First().FoodItemId!);
        if (firstFoodItem == null)
            return BadRequest("FoodItem not found.");

        var restaurantId = firstFoodItem.RestaurantId;
        if (restaurantId == null)
            return BadRequest("Restaurant not found.");

        // Create the order
        var order = new Order
        {
            AppUserId = userId,
            RestaurantId = restaurantId,
            ShippingAddress = shippingAddress,
            Note = orderCreateDto.Note // Assuming you have a Note property
        };

        // Verify all items belong to the same restaurant
        foreach (var itemDto in orderCreateDto.OrderItems)
        {
            var foodItem = await foodItemRepository.GetFoodItemByIdAsync(itemDto.FoodItemId!);
            if (foodItem == null || foodItem.RestaurantId != restaurantId)
                return BadRequest("All items in the order must be from the same restaurant.");

            // Calculate the price including variations
            int totalPrice = foodItem.BasePrice;

            var orderItem = new OrderItem
            {
                OriginalFoodItemId = foodItem.Id,
                FoodName = foodItem.Name,
                FoodDescription = foodItem.Description,
                Quantity = itemDto.Quantity,
                FoodItemPhoto = foodItem.FoodItemPhoto,
                Price = foodItem.BasePrice // Base price
            };

            // Handle selected variations
            foreach (var variationDto in itemDto.SelectedVariations)
            {
                var variation = await variationRepository.GetVariationByIdAsync(variationDto.VariationId);
                if (variation == null)
                    return BadRequest($"Variation {variationDto.VariationId} not found.");

                var variationOption = await variationOptionRepository.GetVariationOptionByIdAsync(variationDto.VariationOptionId);
                if (variationOption == null || variationOption.VariationId != variation.Id)
                    return BadRequest($"Variation Option {variationDto.VariationOptionId} not found or does not belong to Variation {variation.Id}.");

                // Add the price adjustment
                totalPrice += variationOption.PriceAdjustment;

                // Add to OrderItemVariations
                var orderItemVariation = new OrderItemVariation
                {
                    VariationId = variation.Id,
                    VariationOptionId = variationOption.Id
                };

                orderItem.OrderItemVariations.Add(orderItemVariation);
            }

            // Update the price with adjustments
            orderItem.Price = totalPrice;

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
}
