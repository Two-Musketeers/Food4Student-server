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
    [HttpPost("restaurants/{restaurantId}")]
    public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] OrderCreateDto orderCreateDto, string restaurantId)
    {
        // Validate Restaurant
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(restaurantId);
        if (restaurant == null)
            return NotFound(new { message = "Restaurant not found." });

        // Get User ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { message = "Invalid user." });

        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null)
            return Unauthorized(new { message = "User not found." });

        if (orderCreateDto.PhoneNumber == null) {
            orderCreateDto.PhoneNumber = user.PhoneNumber;
        }
        // Initialize Order
        var order = new Order
        {
            AppUserId = userId,
            RestaurantId = restaurantId,
            ShippingAddress = orderCreateDto.ShippingAddress,
            Latitude = orderCreateDto.Latitude,
            Longitude = orderCreateDto.Longitude,
            PhoneNumber = orderCreateDto.PhoneNumber,
            Name = orderCreateDto.Name,
            Note = orderCreateDto.Note
        };

        // Collect all VariationIds and VariationOptionIds for bulk fetching
        var allVariationIds = orderCreateDto.OrderItems
            .Where(oi => oi.SelectedVariations != null && oi.SelectedVariations.Any())
            .SelectMany(oi => oi.SelectedVariations.Select(v => v.VariationId))
            .Distinct()
            .ToList();

        var allVariationOptionIds = orderCreateDto.OrderItems
            .Where(oi => oi.SelectedVariations != null && oi.SelectedVariations.Any())
            .SelectMany(oi => oi.SelectedVariations.SelectMany(v => v.VariationOptionIds))
            .Distinct()
            .ToList();

        // Fetch Variations and VariationOptions in bulk
        var variations = await variationRepository.GetVariationsByIdsAsync(allVariationIds);
        var variationOptions = await variationOptionRepository.GetVariationOptionsByIdsAsync(allVariationOptionIds);

        // Convert to dictionaries for quick lookup
        var variationDict = variations.ToDictionary(v => v.Id, v => v);
        var variationOptionDict = variationOptions.ToDictionary(vo => vo.Id, vo => vo);

        foreach (var itemDto in orderCreateDto.OrderItems)
        {
            // Validate Quantity and FoodItemId
            if (itemDto.Quantity <= 0)
                return BadRequest(new { message = "Quantity must be greater than 0." });
            if (string.IsNullOrEmpty(itemDto.FoodItemId))
                return BadRequest(new { message = "Invalid food item id." });

            // Validate FoodItem and its association with the Restaurant
            var foodItem = await foodItemRepository.GetFoodItemWithCategoryAsync(itemDto.FoodItemId);
            if (foodItem == null || foodItem.FoodCategory.RestaurantId != restaurantId)
                return BadRequest(new { message = $"FoodItem with ID {itemDto.FoodItemId} not found in the specified restaurant." });

            // Initialize Variations List and Price Adjustment
            var variationsList = new List<string>();
            int variationOptionPriceAdjustment = 0;

            if (itemDto.SelectedVariations != null && itemDto.SelectedVariations.Count > 0)
            {
                // Group SelectedVariations by VariationId to enforce count constraints
                var groupedVariations = itemDto.SelectedVariations
                    .GroupBy(v => v.VariationId)
                    .ToList();

                foreach (var variationGroup in groupedVariations)
                {
                    var variationId = variationGroup.Key;
                    var selectedOptionIds = variationGroup.SelectMany(v => v.VariationOptionIds).ToList(); // Flatten all VariationOptionIds

                    // Validate Variation Exists and Belongs to FoodItem
                    if (!variationDict.TryGetValue(variationId, out var variation) || variation.FoodItemId != foodItem.Id)
                        return BadRequest(new { message = $"Invalid Variation ID: {variationId} for FoodItem ID: {foodItem.Id}." });

                    // Enforce Min and Max Select Constraints
                    if (selectedOptionIds.Count < variation.MinSelect)
                        return BadRequest(new { message = $"Variation '{variation.Name}' requires at least {variation.MinSelect} selections." });

                    if (variation.MaxSelect.HasValue && selectedOptionIds.Count > variation.MaxSelect.Value)
                        return BadRequest(new { message = $"Variation '{variation.Name}' allows a maximum of {variation.MaxSelect.Value} selections." });

                    // Validate VariationOptions
                    var validVariationOptions = selectedOptionIds
                        .Where(variationOptionDict.ContainsKey)
                        .Select(variationOptionId => variationOptionDict[variationOptionId])
                        .ToList();

                    if (validVariationOptions.Count != selectedOptionIds.Count)
                        return BadRequest(new { message = "One or more VariationOptionIds are invalid." });

                    foreach (var variationOption in validVariationOptions)
                    {
                        if (variationOption.VariationId != variationId)
                            return BadRequest(new { message = $"VariationOption ID: {variationOption.Id} does not belong to Variation ID: {variationId}." });

                        variationsList.Add($"{variation.Name} - {variationOption.Name}");
                        variationOptionPriceAdjustment += variationOption.PriceAdjustment;
                    }
                }
            }

            var variationsString = variationsList.Any() ? string.Join(", ", variationsList) : null;

            var orderItem = new OrderItem
            {
                FoodName = foodItem.Name,
                FoodDescription = foodItem.Description,
                Price = foodItem.BasePrice + variationOptionPriceAdjustment,
                Quantity = itemDto.Quantity,
                FoodItemPhotoUrl = foodItem.FoodItemPhoto?.Url,
                OriginalFoodItemId = foodItem.Id,
                Variations = variationsString
            };

            order.OrderItems.Add(orderItem);
        }

        // Calculate Total Price
        order.TotalPrice = order.OrderItems.Sum(oi => oi.Price * oi.Quantity);

        // Add and Save Order
        orderRepository.AddOrder(order);
        var isSaved = await orderRepository.SaveAllAsync();

        if (!isSaved)
            return BadRequest(new { message = "Failed to create order." });

        // Map to OrderDto
        var orderDto = mapper.Map<OrderDto>(order);

        // Set Restaurant Name
        orderDto.RestaurantName = restaurant.Name;

        var restaurantOwner = await userRepository.GetUserByIdAsync(restaurantId);
        restaurantOwner.UserNotifications.Add(new UserNotification
        {
            Title = "Bạn có đơn hàng mới",
            Content = "Một người dùng đã tạo đơn hàng mới tại quán của bạn.",
            Image = restaurant.Logo?.Url,
            Timestamp = DateTime.UtcNow,
            IsUnread = true,
        });

        var restaurantOwnerDeviceToken = await userRepository.GetDeviceTokens(restaurantId);
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
        var result = await orderRepository.SaveAllAsync();
        if (!result)
            return BadRequest(new { message = "Failed to create order." });
        return CreatedAtAction(nameof(GetOrderById), new { id = orderDto.Id }, orderDto);
    }
}