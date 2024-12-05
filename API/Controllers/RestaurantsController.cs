using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class RestaurantsController(IRestaurantRepository restaurantRepository,
        IMapper mapper, IUserRepository userRepository, IPhotoService photoService,
        IFoodItemRepository foodItemRepository, IRatingRepository ratingRepository,
        IFoodCategoryRepository foodCategoryRepository) : BaseApiController
{
    [Authorize(Policy = "RequireUserRole")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants([FromQuery] RestaurantParams restaurantParams)
    {
        var restaurants = await restaurantRepository.GetApprovedRestaurantsAsync(restaurantParams);

        var restaurantsToReturn = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

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

        var restaurant = new Restaurant
        {
            Id = userId,
            Name = restaurantRegisterDto.Name,
            Description = restaurantRegisterDto.Description,
            Address = restaurantRegisterDto.Address,
            Latitude = restaurantRegisterDto.Latitude,
            Longitude = restaurantRegisterDto.Longitude,
        };

        user.OwnedRestaurant = restaurant;

        if (await restaurantRepository.SaveAllAsync())
        {
            var restaurantToReturn = mapper.Map<RestaurantDto>(restaurant);
            return CreatedAtAction(nameof(GetOwnedRestaurant), new { id = restaurant.Id }, restaurantToReturn);
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

        restaurant.Name = restaurantUpdateDto.Name;
        restaurant.Description = restaurantUpdateDto.Description;
        restaurant.Address = restaurantUpdateDto.Address;
        restaurant.Latitude = restaurantUpdateDto.Latitude;
        restaurant.Longitude = restaurantUpdateDto.Longitude;

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
    public async Task<ActionResult<FoodItemDto>> CreateFoodItem(string foodCategoryId, FoodItemRegisterDto foodItemCreateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        // Retrieve the specific FoodCategory
        var category = await foodCategoryRepository.GetFoodCategoryAsync(foodCategoryId);
        if (category == null || category.RestaurantId != user.OwnedRestaurant.Id)
            return BadRequest("Invalid food category");

        var foodItem = mapper.Map<FoodItem>(foodItemCreateDto);
        foodItem.FoodCategory.RestaurantId = user.OwnedRestaurant.Id;
        foodItem.FoodCategoryId = category.Id;

        // Add the FoodItem to the specific category
        category.FoodItems.Add(foodItem);

        foodItemRepository.AddFoodItem(foodItem);

        if (await foodItemRepository.SaveAllAsync())
        {
            var foodItemDto = mapper.Map<FoodItemDto>(foodItem);
            return CreatedAtAction(nameof(GetFoodItemById), new { id = foodItem.Id }, foodItemDto);
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

    // **New Endpoint: Get FoodItem by ID Directly**
    [Authorize(Policy = "RequireUserRole")]
    [HttpGet("food-items/{id}")]
    public async Task<ActionResult<FoodItemDto>> GetFoodItemByIdDirectly(string id)
    {
        var foodItem = await foodItemRepository.GetFoodItemByIdDirectlyAsync(id);
        if (foodItem == null)
            return NotFound("Food item not found.");

        var foodItemDto = mapper.Map<FoodItemDto>(foodItem);
        return Ok(foodItemDto);
    }

    //Will use it for restaurant Owner
    [Authorize(Policy = "RequireUserRole")]
    [HttpGet("{restaurantId}/food-items")]
    public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetAllFoodItemsByRestaurant(string restaurantId)
    {
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(restaurantId);
        if (restaurant == null)
            return NotFound("Restaurant not found.");

        var foodItems = await foodItemRepository.GetFoodItemsByRestaurantIdAsync(restaurantId);
        var foodItemsDto = mapper.Map<IEnumerable<FoodItemDto>>(foodItems);
        return Ok(foodItemsDto);
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpGet("{restaurantId}/food-categories")]
    public async Task<ActionResult<IEnumerable<FoodCategoryDto>>> GetFoodCategoriesWithItems(string restaurantId)
    {
        var restaurant = await restaurantRepository.GetRestaurantWithCategoriesAsync(restaurantId);
        if (restaurant == null)
            return NotFound("Restaurant not found.");

        var categories = restaurant.FoodCategories;

        var categoriesDto = mapper.Map<IEnumerable<FoodCategoryDto>>(categories);
        return Ok(categoriesDto);
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete("food-categories/{foodCategoryId}/food-items/{id}")]
    public async Task<ActionResult> DeleteFoodItem(string foodCategoryId, string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        var category = await foodCategoryRepository.GetFoodCategoryAsync(foodCategoryId);
        if (category == null || category.RestaurantId != user.OwnedRestaurant.Id)
            return BadRequest("Invalid food category");

        var foodItem = await foodItemRepository.GetFoodItemByIdAsync(id);

        if (foodItem == null || foodItem.FoodCategoryId != foodCategoryId)
            return NotFound("Food item not found in the specified category");

        // Remove the FoodItem from the category
        foodCategoryRepository.RemoveFoodItemFromCategory(category, foodItem);

        // Optionally delete the FoodItem from the database
        foodItemRepository.DeleteFoodItem(foodItem);

        if (await foodItemRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to delete food item");
    }

    // Photo controller
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("photos/logo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        // Delete the old photo
        if (user.OwnedRestaurant.Logo != null && user.OwnedRestaurant.Logo.PublicId != null)
        {
            var deleteResult = await photoService.DeletePhotoAsync(user.OwnedRestaurant.Logo.PublicId);

            if (deleteResult.Error != null) return BadRequest(deleteResult.Error.Message);
        }

        var result = await photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        user.OwnedRestaurant.Logo = photo;

        if (await restaurantRepository.SaveAllAsync())
            return CreatedAtAction(nameof(GetOwnedRestaurant), new { id = user.OwnedRestaurant.Id }, mapper.Map<PhotoDto>(photo));

        return BadRequest("Problem adding photo");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("photos/banner")]
    public async Task<ActionResult<PhotoDto>> AddBannerPhoto(IFormFile file)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        // Delete the old photo
        if (user.OwnedRestaurant.Banner != null && user.OwnedRestaurant.Banner.PublicId != null)
        {
            var deleteResult = await photoService.DeletePhotoAsync(user.OwnedRestaurant.Banner.PublicId);

            if (deleteResult.Error != null) return BadRequest(deleteResult.Error.Message);
        }

        var result = await photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        user.OwnedRestaurant.Banner = photo;

        if (await restaurantRepository.SaveAllAsync())
            return CreatedAtAction(nameof(GetOwnedRestaurant), new { id = user.OwnedRestaurant.Id }, mapper.Map<PhotoDto>(photo));

        return BadRequest("Problem adding photo");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("food-categories/{foodCategoryId}/food-items/{foodItemId}/photos")]
    public async Task<ActionResult<PhotoDto>> AddFoodItemPhoto(IFormFile file, string foodCategoryId, string foodItemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        // Retrieve the specific FoodCategory
        var category = await foodCategoryRepository.GetFoodCategoryAsync(foodCategoryId);
        if (category == null || category.RestaurantId != user.OwnedRestaurant.Id)
            return BadRequest("Invalid food category");

        var foodItem = await foodItemRepository.GetFoodItemByIdAsync(foodItemId);
        if (foodItem == null || foodItem.FoodCategoryId != foodCategoryId)
            return BadRequest("Food item not found in the specified category");

        // Delete the old photo
        if (foodItem.FoodItemPhoto != null && foodItem.FoodItemPhoto.PublicId != null)
        {
            var deleteResult = await photoService.DeletePhotoAsync(foodItem.FoodItemPhoto.PublicId);

            if (deleteResult.Error != null) return BadRequest(deleteResult.Error.Message);
        }

        var result = await photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        foodItem.FoodItemPhoto = photo;

        if (await foodItemRepository.SaveAllAsync())
            return CreatedAtAction(nameof(GetOwnedRestaurant), new { id = user.OwnedRestaurant.Id }, mapper.Map<PhotoDto>(photo));

        return BadRequest("Problem adding photo");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete("photos/logo")]
    public async Task<ActionResult> DeleteLogoPhoto()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        var photo = user.OwnedRestaurant.Logo;
        if (photo == null) return BadRequest("Logo photo not found");

        if (photo.PublicId == null) return BadRequest("There's no photo mate");

        var result = await photoService.DeletePhotoAsync(photo.PublicId);

        if (result.Error != null) return BadRequest(result.Error.Message);

        user.OwnedRestaurant.Logo = null;

        if (await restaurantRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting photo");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete("photos/banner")]
    public async Task<ActionResult> DeleteBannerPhoto()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        var photo = user.OwnedRestaurant.Banner;
        if (photo == null) return BadRequest("Banner photo not found");

        if (photo.PublicId == null) return BadRequest("There's no photo mate");

        var result = await photoService.DeletePhotoAsync(photo.PublicId);

        if (result.Error != null) return BadRequest(result.Error.Message);

        user.OwnedRestaurant.Banner = null;

        if (await restaurantRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting photo");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete("food-categories/{foodCategoryId}/food-items/{foodItemId}/photos")]
    public async Task<ActionResult> DeleteFoodItemPhoto(string foodCategoryId, string foodItemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        // Retrieve the specific FoodCategory
        var category = await foodCategoryRepository.GetFoodCategoryAsync(foodCategoryId);
        if (category == null || category.RestaurantId != user.OwnedRestaurant.Id)
            return BadRequest("Invalid food category");

        var foodItem = await foodItemRepository.GetFoodItemByIdAsync(foodItemId);
        if (foodItem == null || foodItem.FoodCategoryId != foodCategoryId)
            return BadRequest("Food item not found in the specified category");

        var photo = foodItem.FoodItemPhoto;
        if (photo == null) return BadRequest("Food item photo not found");

        if (photo.PublicId == null) return BadRequest("There's no photo mate");

        var result = await photoService.DeletePhotoAsync(photo.PublicId);

        if (result.Error != null) return BadRequest(result.Error.Message);

        foodItem.FoodItemPhoto = null;

        if (await foodItemRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting photo");
    }

    [Authorize]
    [HttpGet("{id}/ratings")]
    public async Task<ActionResult> GetRestaurantRatings(string id)
    {
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(id);

        if (restaurant == null) return NotFound();

        var ratings = await ratingRepository.GetRestaurantRatingsAsync(id);

        var returnRatings = mapper.Map<IEnumerable<RatingDto>>(ratings);

        return Ok(returnRatings);
    }

    [NonAction]
    public async Task<ActionResult<RestaurantDto>> GetOwnedRestaurant()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return NotFound();

        var restaurantToReturn = mapper.Map<RestaurantDto>(user.OwnedRestaurant);

        return Ok(restaurantToReturn);
    }
}