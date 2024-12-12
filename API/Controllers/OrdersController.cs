using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class OrdersController(IMapper mapper,
    IOrderRepository orderRepository,
    IShippingAddressRepository shippingAddressRepository,
    IFoodItemRepository foodItemRepository,
    IRestaurantRepository restaurantRepository,
    IVariationRepository variationRepository,
    IVariationOptionRepository variationOptionRepository,
    IUserRepository userRepository) : BaseApiController
{
    // Get all orders for the authenticated restaurant owner
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var orders = await orderRepository.GetOrdersByRestaurantIdAsync(userId);
        var ordersDto = mapper.Map<IEnumerable<OrderDto>>(orders);
        return Ok(ordersDto);
    }

    // Get specific order by ID
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var order = await orderRepository.GetOrderByIdAsync(id);

        if (order == null)
            return NotFound(new { message = "Order not found." });

        if (order.AppUserId != userId && order.RestaurantId != userId)
            return Unauthorized(new { message = "You do not have access to this order." });

        var orderDto = mapper.Map<OrderDto>(order);
        return Ok(orderDto);
    }

    // Approve order
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPut("{id}/approve-order")]
    public async Task<IActionResult> ApproveOrder(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var order = await orderRepository.GetOrderByIdAsync(id);

        if (order == null)
            return NotFound(new { message = "Order not found." });

        if (order.RestaurantId != userId)
            return Unauthorized(new { message = "You do not have access to approve this order." });

        if (order.Status != "Pending")
            return BadRequest(new { message = "You can only approve pending orders." });

        if (order.AppUserId == null)
            return BadRequest(new { message = "Order does not have a user." });

        var userPlacedOrder = await userRepository.GetUserByIdAsync(order.AppUserId);
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(userId);

        if (userPlacedOrder == null || restaurant == null)
            return BadRequest(new { message = "Failed to approve order." });

        userPlacedOrder.UserNotifications.Add(new UserNotification
        {
            Title = "Đơn hàng đã được chấp nhận",
            Content = $"Đơn hàng của bạn ở quán {restaurant.Name} đã được chấp nhận.",
            Image = restaurant.Logo?.Url,
            Timestamp = DateTime.UtcNow,
            IsUnread = true,
        });

        order.Status = "Approved";

        bool isSaved = await orderRepository.SaveAllAsync();
        if (!isSaved)
            return BadRequest(new { message = "Failed to approve order." });

        return NoContent();
    }

    // Reject order
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPut("{id}/reject-order")]
    public async Task<IActionResult> RejectOrder(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var order = await orderRepository.GetOrderByIdAsync(id);

        if (order == null)
            return NotFound(new { message = "Order not found." });

        if (order.RestaurantId != userId)
            return Unauthorized(new { message = "You do not have access to reject this order." });

        if (order.Status != "Pending")
            return BadRequest(new { message = "You can only reject pending orders." });

        if (order.AppUserId == null)
            return BadRequest(new { message = "Order does not have a user." });

        var userPlacedOrder = await userRepository.GetUserByIdAsync(order.AppUserId);
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(userId);

        if (userPlacedOrder == null || restaurant == null)
            return BadRequest(new { message = "Failed to reject order." });

        userPlacedOrder.UserNotifications.Add(new UserNotification
        {
            Title = "Đơn hàng đã bị từ chối",
            Content = $"Đơn hàng của bạn ở quán ${restaurant.Name} đã bị từ chối.",
            Image = restaurant.Logo?.Url,
            Timestamp = DateTime.UtcNow,
            IsUnread = true,
        });

        order.Status = "Rejected";

        bool isSaved = await orderRepository.SaveAllAsync();
        if (!isSaved)
            return BadRequest(new { message = "Failed to reject order." });

        return NoContent();
    }

    // Deliver order
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPut("{id}/deliver-order")]
    public async Task<IActionResult> DeliverOrder(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var order = await orderRepository.GetOrderByIdAsync(id);

        if (order == null)
            return NotFound(new { message = "Order not found." });

        if (order.RestaurantId != userId)
            return Unauthorized(new { message = "You do not have access to deliver this order." });

        if (order.Status != "Approved")
            return BadRequest(new { message = "You can only deliver approved orders." });

        if (order.AppUserId == null)
            return BadRequest(new { message = "Order does not have a user." });

        var userPlacedOrder = await userRepository.GetUserByIdAsync(order.AppUserId);
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(userId);

        if (userPlacedOrder == null || restaurant == null)
            return BadRequest(new { message = "Failed to deliver order." });

        userPlacedOrder.UserNotifications.Add(new UserNotification
        {
            Title = $"Đơn hàng tại ${restaurant.Name} đã hoàn tất",
            Content = "Cảm ơn bạn đã sử dụng dịch vụ Food4Students. Hãy chia sẻ cảm nhận của bạn về đơn hàng để giúp những khách hàng khác có thể tham khảo nhé",
            Image = restaurant.Logo?.Url,
            Timestamp = DateTime.UtcNow,
            IsUnread = true,
        });

        order.Status = "Delivered";

        bool isSaved = await orderRepository.SaveAllAsync();
        if (!isSaved)
            return BadRequest(new { message = "Failed to deliver order." });

        return NoContent();
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] OrderCreateDto orderCreateDto)
    {
        // Retrieve the authenticated user's ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Validate shipping address
        var shippingAddress = await shippingAddressRepository.GetShippingAddressByIdAsync(orderCreateDto.ShippingInfoId);
        if (shippingAddress == null)
            return BadRequest(new { message = "Invalid shipping address." });

        // Extract all non-null FoodItemIds from the order items
        var foodItemIds = orderCreateDto.OrderItems.Select(oi => oi.FoodItemId).Distinct().ToList();

        // Fetch all corresponding FoodItems with their categories in a single query
        var foodItems = await foodItemRepository.GetFoodItemsWithCategoryAsync(foodItemIds);

        // Check if all food items exist
        if (foodItems.Count() != foodItemIds.Count)
            return BadRequest(new { message = "One or more food items not found." });

        // Verify all food items belong to the specified restaurant
        if (foodItems.Any(fi => fi.FoodCategory.RestaurantId != orderCreateDto.RestaurantId))
            return BadRequest(new { message = "All items in the order must be from the specified restaurant." });

        // Fetch the Restaurant and verify its existence and approval status
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(orderCreateDto.RestaurantId!);
        if (restaurant == null)
            return BadRequest(new { message = "Restaurant not found." });

        if (!restaurant.IsApproved)
            return BadRequest(new { message = "Cannot place an order to an unapproved restaurant." });

        // Initialize the Order entity
        var order = new Order
        {
            AppUserId = userId,
            RestaurantId = orderCreateDto.RestaurantId,
            ShippingAddress = shippingAddress,
            Note = orderCreateDto.Note ?? string.Empty,
        };

        int totalPrice = 0;

        // Process each Order Item
        foreach (var itemDto in orderCreateDto.OrderItems)
        {
            // Retrieve the corresponding FoodItem
            var foodItem = foodItems.First(fi => fi.Id == itemDto.FoodItemId);

            // Initialize the OrderItem entity
            var orderItem = new OrderItem
            {
                OriginalFoodItemId = foodItem.Id,
                FoodName = foodItem.Name,
                FoodDescription = foodItem.Description,
                Quantity = itemDto.Quantity,
                FoodItemPhoto = foodItem.FoodItemPhoto,
                Price = foodItem.BasePrice,
            };

            // Handle Variations if any
            if (itemDto.SelectedVariations != null)
            {
                foreach (var variationDto in itemDto.SelectedVariations)
                {
                    // Validate VariationIds are not null or empty
                    if (string.IsNullOrEmpty(variationDto.VariationId) || string.IsNullOrEmpty(variationDto.VariationOptionId))
                        return BadRequest(new { message = "VariationId and VariationOptionId cannot be null." });

                    // Fetch Variation
                    var variation = await variationRepository.GetVariationByIdAsync(variationDto.VariationId);
                    if (variation == null)
                        return BadRequest(new { message = $"Variation {variationDto.VariationId} not found." });

                    // Fetch VariationOption
                    var variationOption = await variationOptionRepository.GetVariationOptionByIdAsync(variationDto.VariationOptionId);
                    if (variationOption == null)
                        return BadRequest(new { message = $"Variation Option {variationDto.VariationOptionId} not found." });

                    // Ensure VariationOption belongs to the Variation
                    if (variationOption.VariationId != variation.Id)
                        return BadRequest(new { message = $"Variation Option {variationOption.Id} does not belong to Variation {variation.Id}." });

                    // Initialize OrderItemVariation
                    var orderItemVariation = new OrderItemVariation
                    {
                        VariationId = variation.Id,
                        Variation = variation,
                        VariationOptionId = variationOption.Id,
                        VariationOption = variationOption
                    };

                    // Add to OrderItemVariations and adjust price
                    orderItem.OrderItemVariations.Add(orderItemVariation);
                    orderItem.Price += variationOption.PriceAdjustment;
                }
            }

            // Calculate total price for the order
            totalPrice += orderItem.Price * orderItem.Quantity;

            // Add the OrderItem to the Order
            order.OrderItems.Add(orderItem);
        }

        // Assign total price to the Order
        order.TotalPrice = totalPrice;

        // Add the Order to the repository and save changes
        orderRepository.AddOrder(order);
        var result = await orderRepository.SaveAllAsync();

        if (!result) return BadRequest(new { message = "Failed to create order." });

        // Map the Order entity to OrderDto for the response
        var orderDto = mapper.Map<OrderDto>(order);
        orderDto.RestaurantName = restaurant.Name;

        var restaurantOwnerDeviceToken = await userRepository.GetDeviceTokens(orderCreateDto.RestaurantId);
        if (restaurantOwnerDeviceToken.Count > 0)
        {
            var notification = new MulticastMessage
            {
                Tokens = restaurantOwnerDeviceToken,
                Data = new Dictionary<string, string>
                {
                    { "Mã đơn hàng", order.Id },
                    { "Trạng thái đơn hàng", order.Status },
                },
                Notification = new Notification
                {
                    Title = "Bạn có đơn hàng mới",
                    Body = $"Đơn hàng {order.Id} đã được tạo."
                }
            };

            var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(notification);
        }
        var userDeviceToken = await userRepository.GetDeviceTokens(userId!);
        if (userDeviceToken.Count > 0)
        {
            var notification = new MulticastMessage
            {
                Tokens = userDeviceToken,
                Data = new Dictionary<string, string>
                {
                    { "Mã đơn hàng", order.Id },
                    { "Trạng thái đơn hàng", order.Status },
                },
                Notification = new Notification
                {
                    Title = "Đơn hàng của bạn",
                    Body = $"Đơn hàng {order.Id} đã được tạo."
                }
            };

            var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(notification);
        }
        return CreatedAtAction(nameof(GetOrderById), new { id = orderDto.Id }, orderDto);
    }
}