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
        IFoodItemRepository foodItemRepository) : BaseApiController
{
    [Authorize(Policy = "RequireUserRole")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants([FromQuery] RestaurantParams restaurantParams)
    {
        var restaurants = await restaurantRepository.GetRestaurantsAsync(restaurantParams);

        var restaurantsToReturn = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        return Ok(restaurantsToReturn);
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpGet("{id}")]
    public async Task<ActionResult<RestaurantDto>> GetRestaurantById(string id)
    {
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(id);

        if (restaurant == null) return NotFound();

        var restaurantToReturn = mapper.Map<RestaurantDto>(restaurant);

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

    //FoodItem controller
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("food-items")]
    public async Task<ActionResult<FoodItemDto>> CreateFoodItem(FoodItemRegisterDto foodItemCreateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        var foodItem = mapper.Map<FoodItem>(foodItemCreateDto);

        user.OwnedRestaurant.Menu.Add(foodItem);

        if (await foodItemRepository.SaveAllAsync())
        {
            var foodItemDto = mapper.Map<FoodItemDto>(foodItem);
            return CreatedAtAction(nameof(GetFoodItems), new { id = foodItem.Id }, foodItemDto);
        }

        return BadRequest("Failed to create food item");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPut("food-items/{foodItemId}")]
    public async Task<ActionResult> UpdateFoodItem(string foodItemId, FoodItemDto foodItemDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (foodItemId == null) return NotFound("Id is required");

        var foodItem = await foodItemRepository.GetFoodItemByIdAsync(foodItemId);

        if (foodItem == null) return NotFound("Food item not found");

        // Ensure the food item belongs to the authenticated user
        if (foodItem.RestaurantId != userId) return Forbid();

        mapper.Map(foodItemDto, foodItem);

        var result = await foodItemRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to update food item");

        return Ok("Food item has been updated successfully");
    }

    //The get restaurant by id already contain the menu so i just make this in case
    [Authorize(Policy = "RequireUserRole")]
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
    [HttpDelete("food-items/{id}")]
    public async Task<ActionResult> DeleteFoodItem(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        var foodItem = await foodItemRepository.GetFoodItemByIdAsync(id);

        if (foodItem == null)
            return NotFound();

        if (foodItem.RestaurantId != user.OwnedRestaurant.Id)
            return Unauthorized("You do not own this food item");

        user.OwnedRestaurant.Menu.Remove(foodItem);

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

        //Delete the old photo
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

        if (await restaurantRepository.SaveAllAsync()) return CreatedAtAction(nameof(GetOwnedRestaurant), new { id = user.OwnedRestaurant.Id }, mapper.Map<PhotoDto>(photo));

        return BadRequest("Problem adding photo");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("photos/banner")]
    public async Task<ActionResult<PhotoDto>> AddBannerPhoto(IFormFile file)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        //Delete the old photo
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

        if (await restaurantRepository.SaveAllAsync()) return CreatedAtAction(nameof(GetOwnedRestaurant), new { id = user.OwnedRestaurant.Id }, mapper.Map<PhotoDto>(photo));

        return BadRequest("Problem adding photo");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("food-items/{foodItemId}/photos")]
    public async Task<ActionResult<PhotoDto>> AddFoodItemPhoto(IFormFile file, string foodItemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        var foodItem = user.OwnedRestaurant.Menu.FirstOrDefault(f => f.Id == foodItemId);

        if (foodItem == null) return BadRequest("Food item not found");

        //Delete the old photo
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

        if (await restaurantRepository.SaveAllAsync()) return CreatedAtAction(nameof(GetOwnedRestaurant), new { id = user.OwnedRestaurant.Id }, mapper.Map<PhotoDto>(photo));

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
    [HttpDelete("fooditems/{foodItemId}/photos")]
    public async Task<ActionResult> DeleteFoodItemPhoto(string foodItemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        var foodItem = user.OwnedRestaurant.Menu.FirstOrDefault(f => f.Id == foodItemId);

        if (foodItem == null) return BadRequest("Food item not found");

        var photo = foodItem.FoodItemPhoto;
        if (photo == null) return BadRequest("Food item photo not found");

        if (photo.PublicId == null) return BadRequest("There's no photo mate");

        var result = await photoService.DeletePhotoAsync(photo.PublicId);

        if (result.Error != null) return BadRequest(result.Error.Message);

        foodItem.FoodItemPhoto = null;

        if (await restaurantRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting photo");
    }

    [Authorize(Policy = "RequireUserRole")]
    [HttpGet("{id}/ratings")]
    public async Task<ActionResult> GetRestaurantRatings(string id)
    {
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(id);

        if (restaurant == null) return NotFound();

        var ratings = restaurant.Ratings;

        return Ok(ratings);
    }

    // Helper methods
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

