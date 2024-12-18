using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController(IRestaurantRepository restaurantRepository,
        IMapper mapper, IUserRepository userRepository,
        IFoodItemRepository foodItemRepository, IRatingRepository ratingRepository,
        IFoodCategoryRepository foodCategoryRepository) : ControllerBase
{
    [Authorize(Policy = "RequireUserRole")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants(
        [FromQuery] PaginationParams restaurantParams,
        [FromBody] CurrentLocationDto currentLocationDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userRepository.GetUserByIdAsync(userId!);

        // Create a HashSet of favorite restaurant IDs for efficient lookup
        var favoriteRestaurantIds = user.FavoriteRestaurants
            .Select(fr => fr.LikedRestaurantId)
            .ToHashSet();

        // Fetch nearby restaurants
        var restaurants = await restaurantRepository.GetNearbyRestaurantsAsync(
            currentLocationDto.Latitude,
            currentLocationDto.Longitude,
            restaurantParams.PageSize,
            restaurantParams.PageNumber);

        // Map to DTOs and set IsFavorited property
        var restaurantsToReturn = restaurants.Select(r => new RestaurantDto
        {
            Id = r.Id!,
            Name = r.Name,
            Description = r.Description,
            Address = r.Address,
            Latitude = r.Location.Y,
            Longitude = r.Location.X,
            LogoUrl = r.Logo?.Url,
            BannerUrl = r.Banner?.Url,
            TotalRatings = r.Ratings.Count,
            AverageRating = r.Ratings.Count != 0 ? r.Ratings.Average(rt => rt.Stars) : 0,
            IsFavorited = favoriteRestaurantIds.Contains(r.Id!)
        }).ToList();

        return Ok(restaurantsToReturn);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<RestaurantDetailDto>> GetRestaurantById(string id)
    {
        var restaurant = await restaurantRepository.GetRestaurantWithDetailsAsync(id);

        if (restaurant == null) return NotFound();

        var restaurantToReturn = mapper.Map<RestaurantDetailDto>(restaurant);

        return Ok(restaurantToReturn);
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost]
    public async Task<ActionResult<RestaurantDto>> AddRestaurant(RestaurantRegisterDto restaurantRegisterDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant != null) return BadRequest("User already owns a restaurant");

        var userLocation = new Point(restaurantRegisterDto.Longitude, restaurantRegisterDto.Latitude) { SRID = 4326 };

        var restaurant = new Restaurant
        {
            Id = userId,
            Name = restaurantRegisterDto.Name,
            Description = restaurantRegisterDto.Description,
            Address = restaurantRegisterDto.Address,
            Location = userLocation
        };

        user.OwnedRestaurant = restaurant;

        if (await restaurantRepository.SaveAllAsync())
        {
            var restaurantToReturn = mapper.Map<RestaurantDto>(restaurant);
            return Ok(restaurantToReturn);
        }

        return BadRequest("Problem adding restaurant");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPut]
    public async Task<ActionResult<RestaurantDto>> UpdateRestaurant(RestaurantRegisterDto restaurantUpdateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        var restaurant = user.OwnedRestaurant;

        var userLocation = new Point(restaurantUpdateDto.Longitude, restaurantUpdateDto.Latitude) { SRID = 4326 };

        restaurant.Name = restaurantUpdateDto.Name;
        restaurant.Description = restaurantUpdateDto.Description;
        restaurant.Address = restaurantUpdateDto.Address;
        restaurant.Location = userLocation;

        await restaurantRepository.SaveAllAsync();

        return Ok(mapper.Map<RestaurantDto>(restaurant));
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete]
    public async Task<ActionResult> DeleteRestaurant()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        restaurantRepository.DeleteRestaurant(user.OwnedRestaurant);

        await restaurantRepository.SaveAllAsync();

        return NoContent();
    }

    // FoodItem controller
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("food-categories/{foodCategoryId}/food-items")]
    public async Task<ActionResult<FoodItemDto>> CreateFoodItem(FoodItemRegisterDto foodItemCreateDto, string foodCategoryId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        // Ensure the category exists
        var category = await foodCategoryRepository.GetFoodCategoryAsync(foodCategoryId);
        if (category == null || category.RestaurantId != user.OwnedRestaurant.Id)
            return BadRequest("Invalid food category");

        var foodItem = mapper.Map<FoodItem>(foodItemCreateDto);
        foodItem.FoodCategoryId = category.Id;

        // Add the FoodItem to the specific category
        category.FoodItems.Add(foodItem);

        foodItemRepository.AddFoodItem(foodItem);

        if (await foodItemRepository.SaveAllAsync())
        {
            var foodItemDto = mapper.Map<FoodItemDto>(foodItem);
            return Ok(foodItemDto);
        }

        return BadRequest("Failed to create food item");
    }

    // The get restaurant by id already contains the menu, so this is just in case
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpGet("food-items")]
    public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetFoodItems()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        var foodItems = await foodItemRepository.GetFoodItemsByRestaurantIdAsync(user.Id);

        var foodItemsToReturn = mapper.Map<IEnumerable<FoodItemDto>>(foodItems);

        return Ok(foodItemsToReturn);
    }

    // Again since you already fetch all of the restaurant details in the get restaurant by id, this is just in case
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpGet("food-categories/{foodCategoryId}/food-items")]
    public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetFoodItemsByCategory(string foodCategoryId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        var category = await foodCategoryRepository.GetFoodCategoryAsync(foodCategoryId);
        if (category == null || category.RestaurantId != user.OwnedRestaurant.Id)
            return NotFound("Food category not found");

        var foodItems = await foodItemRepository.GetFoodItemsByCategoryIdAsync(foodCategoryId);

        var foodItemsToReturn = mapper.Map<IEnumerable<FoodItemDto>>(foodItems);

        return Ok(foodItemsToReturn);
    }

    // Just let this in here
    [Authorize(Policy = "RequireUserRole")]
    [HttpGet("food-categories/{foodCategoryId}/food-items/{id}")]
    public async Task<ActionResult<FoodItemDto>> GetFoodItemById(string foodCategoryId, string id)
    {
        var category = await foodCategoryRepository.GetFoodCategoryAsync(foodCategoryId);
        if (category == null)
            return NotFound("Food category not found");

        var foodItem = await foodItemRepository.GetFoodItemByIdAsync(id);
        if (foodItem == null || foodItem.FoodCategoryId != foodCategoryId)
            return NotFound("Food item not found in the specified category");

        var foodItemDto = mapper.Map<FoodItemDto>(foodItem);
        return Ok(foodItemDto);
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete("food-categories/{foodCategoryId}/food-items/{id}")]
    public async Task<ActionResult> DeleteFoodItem(string id, string foodCategoryId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        var foodCategory = await foodCategoryRepository.GetFoodCategoryAsync(foodCategoryId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        if (foodCategory == null || foodCategory.RestaurantId != user.OwnedRestaurant.Id)
            return NotFound("Food category not found");

        if (foodCategory.RestaurantId != userId)
            return BadRequest("Invalid food category");

        var foodItem = await foodItemRepository.GetFoodItemByIdAsync(id);

        if (foodItem == null || foodItem.FoodCategoryId != foodCategoryId)
            return NotFound("Food item not found in the specified category");

        foodItemRepository.DeleteFoodItem(foodItem);

        if (await foodItemRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to delete food item");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPut("food-categories/{foodCategoryId}/food-items/{id}")]
    public async Task<ActionResult<FoodItemDto>> UpdateFoodItem(string id, FoodItemRegisterDto foodItemUpdateDto, string foodCategoryId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        var foodCategory = await foodCategoryRepository.GetFoodCategoryAsync(foodCategoryId);

        if (foodCategory == null || foodCategory.RestaurantId != user.OwnedRestaurant.Id)
            return NotFound("Food category not found");

        var foodItem = await foodItemRepository.GetFoodItemByIdAsync(id);

        if (foodItem == null || foodItem.FoodCategoryId != foodCategoryId)
            return NotFound("Food item not found in the specified category");

        mapper.Map(foodItemUpdateDto, foodItem);

        if (await foodItemRepository.SaveAllAsync())
            return Ok(mapper.Map<FoodItemDto>(foodItem));

        return BadRequest("Failed to update food item");
    }

    [Authorize]
    [HttpGet("{id}/ratings")]
    public async Task<ActionResult<IEnumerable<RatingDto>>> GetRestaurantRatings(string id)
    {
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(id);

        if (restaurant == null) return NotFound();

        var ratings = await ratingRepository.GetRestaurantRatingsAsync(id);

        var returnRatings = mapper.Map<IEnumerable<RatingDto>>(ratings);

        return Ok(returnRatings);
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpGet("owned")]
    public async Task<ActionResult<RestaurantDetailDto>> GetOwnedRestaurant()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return NotFound();

        var ownedRestaurant = await restaurantRepository.GetRestaurantWithDetailsAsync(user.Id);

        var restaurantToReturn = mapper.Map<RestaurantDetailDto>(ownedRestaurant);

        return Ok(restaurantToReturn);
    }
}