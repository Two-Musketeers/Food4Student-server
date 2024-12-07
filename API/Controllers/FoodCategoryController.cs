using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/restaurants/[controller]")]
[ApiController]
public class FoodCategoryController(IUserRepository userRepository,
    IFoodCategoryRepository foodCategoryRepository,
    IMapper mapper,
    IRestaurantRepository restaurantRepository,
    IFoodItemRepository foodItemRepository) : ControllerBase
{
    //Category endpoint
    /// <summary>
    /// Adds a new Food Category to the owner's restaurant.
    /// </summary>
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("food-categories")]
    public async Task<ActionResult<FoodCategoryDto>> AddFoodCategory(FoodCategoryCreateDto foodCategoryCreateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userRepository.GetUserByIdAsync(userId!);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        if (string.IsNullOrWhiteSpace(foodCategoryCreateDto.Name))
            return BadRequest("Category name is required");

        var foodCategory = new FoodCategory
        {
            Name = foodCategoryCreateDto.Name,
            RestaurantId = user.OwnedRestaurant.Id
        };

        foodCategoryRepository.AddFoodCategory(foodCategory);

        if (await foodCategoryRepository.SaveAllAsync())
        {
            var foodCategoryDto = mapper.Map<FoodCategoryDto>(foodCategory);
            return CreatedAtAction(nameof(GetFoodCategoryById), new { id = foodCategory.Id }, foodCategoryDto);
        }

        return BadRequest("Failed to create food category");
    }

    /// <summary>
    /// Retrieves a specific Food Category by ID.
    /// </summary>
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpGet("food-categories/{id}")]
    public async Task<ActionResult<FoodCategoryDto>> GetFoodCategoryById(string id)
    {
        var foodCategory = await foodCategoryRepository.GetFoodCategoryAsync(id);

        if (foodCategory == null)
            return NotFound("Food category not found");

        var foodCategoryDto = mapper.Map<FoodCategoryDto>(foodCategory);
        return Ok(foodCategoryDto);
    }

    /// <summary>
    /// Retrieves all Food Categories for the owner's restaurant.
    /// </summary>
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpGet("food-categories")]
    public async Task<ActionResult<IEnumerable<FoodCategoryDto>>> GetFoodCategories()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var ownedRestaurant = await restaurantRepository.GetRestaurantWithDetailsAsync(userId!);

        if (ownedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        var foodCategories = await foodCategoryRepository.GetFoodCategoriesAsync(ownedRestaurant.Id!);

        var foodCategoriesDto = mapper.Map<IEnumerable<FoodCategoryDto>>(foodCategories);
        return Ok(foodCategoriesDto);
    }

    /// <summary>
    /// Updates an existing Food Category.
    /// </summary>
    [HttpPut("food-categories/{id}")]
    public async Task<ActionResult<FoodCategoryDto>> UpdateFoodCategory(string id, FoodCategoryCreateDto foodCategoryUpdateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userRepository.GetUserByIdAsync(userId!);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        var foodCategory = await foodCategoryRepository.GetFoodCategoryAsync(id);

        if (foodCategory == null || foodCategory.RestaurantId != user.OwnedRestaurant.Id)
            return NotFound("Food category not found");

        if (string.IsNullOrWhiteSpace(foodCategoryUpdateDto.Name))
            return BadRequest("Category name is required");

        foodCategory.Name = foodCategoryUpdateDto.Name;

        if (await foodCategoryRepository.SaveAllAsync())
        {
            var foodCategoryDto = mapper.Map<FoodCategoryDto>(foodCategory);
            return Ok(foodCategoryDto);
        }

        return BadRequest("Failed to update food category");
    }

    /// <summary>
    /// Deletes a Food Category and optionally handles Food Items within it.
    /// </summary>
    [HttpDelete("food-categories/{id}")]
    public async Task<ActionResult> DeleteFoodCategory(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userRepository.GetUserByIdAsync(userId!);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        var foodCategory = await foodCategoryRepository.GetFoodCategoryAsync(id);

        if (foodCategory == null || foodCategory.RestaurantId != user.OwnedRestaurant.Id)
            return NotFound("Food category not found");

        foodCategoryRepository.RemoveFoodCategory(foodCategory);

        if (await foodCategoryRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to delete food category");
    }

    /// <summary>
    /// Migrates a Food Item to a different Food Category.
    /// </summary>
    [HttpPost("food-categories/{foodCategoryId}/migrate-food-item/{foodItemId}")]
    public async Task<ActionResult> MigrateFoodItem(string foodCategoryId, string foodItemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userRepository.GetUserByIdAsync(userId!);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        var targetCategory = await foodCategoryRepository.GetFoodCategoryAsync(foodCategoryId);
        if (targetCategory == null || targetCategory.RestaurantId != user.OwnedRestaurant.Id)
            return NotFound("Target food category not found");

        var foodItem = await foodItemRepository.GetFoodItemByIdAsync(foodItemId);
        if (foodItem == null)
            return NotFound("Food item not found");

        foodItem.FoodCategoryId = targetCategory.Id;

        if (await foodCategoryRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to migrate food item");
    }
}
