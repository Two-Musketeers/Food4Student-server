using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/restaurants")]
public class VariationsController(
    IVariationOptionRepository variationOptionRepository,
    IVariationRepository variationRepository,
    IUserRepository userRepository,
    IMapper mapper,
    IFoodCategoryRepository foodCategoryRepository,
    IFoodItemRepository foodItemRepository) : ControllerBase
{
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("food-categories/{foodCategoryId}/food-items/{foodItemId}/variations")]
    public async Task<ActionResult<VariationDto>> CreateVariation([FromBody] VariationCreateDto variationCreateDto, string foodItemId, string foodCategoryId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        var foodCategory = await foodCategoryRepository.GetFoodCategoryAsync(foodCategoryId);

        if (foodCategory == null)
            return BadRequest("Invalid food category");

        if (foodCategory.RestaurantId != userId)
            return Unauthorized("You do not own this food category");

        if (foodCategory.FoodItems.All(fi => fi.Id != foodItemId))
            return BadRequest("Invalid food item");

        var foodItem = await foodItemRepository.GetFoodItemByIdAsync(foodItemId);

        if (foodItem == null)
            return BadRequest("Invalid food item");

        var variation = mapper.Map<Variation>(variationCreateDto);
        variation.FoodItemId = foodItemId;

        foodItem.Variations.Add(variation);

        variationRepository.AddVariation(variation);

        if (await variationRepository.SaveAllAsync())
        {
            var variationDto = new VariationDto
            {
                Id = variation.Id,
                Name = variation.Name,
                MinSelect = variation.MinSelect,
                MaxSelect = variation.MaxSelect,
            };
            return Ok(variationDto);
        }

        return BadRequest("Failed to create variation");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPut("food-categories/{foodCategoryId}/food-items/{foodItemId}/variations/{id}")]
    public async Task<ActionResult> UpdateVariation(string id, VariationCreateDto variationUpdateDto, string foodItemId, string foodCategoryId)
    {
        var variation = await variationRepository.GetVariationByIdAsync(id);
        if (variation == null)
            return NotFound("Variation not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        if (variation.FoodItemId != foodItemId)
            return BadRequest("Variation does not belong to this food item");

        if (userId != variation.FoodItem.FoodCategory?.RestaurantId)
            return Unauthorized("You do not own this variation");

        mapper.Map(variationUpdateDto, variation);

        if (await variationRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to update variation");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete("food-categories/{foodCategoryId}/food-items/{foodItemId}/variations/{id}")]
    public async Task<ActionResult> DeleteVariation(string id, string foodItemId, string foodCategoryId)
    {
        var variation = await variationRepository.GetVariationByIdAsync(id);
        if (variation == null)
            return NotFound("Variation not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        if (variation.FoodItemId != foodItemId)
            return BadRequest("Variation does not belong to this food item");

        if (userId != variation.FoodItem.FoodCategory?.RestaurantId)
            return Unauthorized("You do not own this variation");

        variationRepository.RemoveVariation(variation);

        if (await variationRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to delete variation");
    }

    // VariationOption Controller
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("food-categories/{foodCategoryId}/food-items/{foodItemId}/variations/{variationId}/variation-options")]
    public async Task<ActionResult<VariationOptionDto>> CreateVariationOption(string variationId, [FromBody] VariationOptionCreateDto optionCreateDto, string foodItemId, string foodCategoryId)
    {
        var variation = await variationRepository.GetVariationByIdAsync(variationId);
        if (variation == null)
            return NotFound("Variation not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        if (variation.FoodItemId != foodItemId)
            return BadRequest("Variation does not belong to this food item");

        if (userId != variation.FoodItem.FoodCategory?.RestaurantId)
            return Unauthorized("You do not own this variation");

        var option = mapper.Map<VariationOption>(optionCreateDto);
        option.VariationId = variationId;

        variation.VariationOptions.Add(option);

        variationOptionRepository.AddVariationOption(option);

        if (await variationOptionRepository.SaveAllAsync())
        {
            var optionToReturn = mapper.Map<VariationOptionDto>(option);
            return Ok(optionToReturn);
        }

        return BadRequest("Failed to create variation option");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPut("food-categories/{foodCategoryId}/food-items/{foodItemId}/variations/{variationId}/variation-options/{id}")]
    public async Task<ActionResult<VariationOptionDto>> UpdateVariationOption(string id, string variationId, VariationOptionCreateDto optionUpdateDto, string foodItemId, string foodCategoryId)
    {
        var option = await variationOptionRepository.GetVariationOptionByIdAsync(id);
        if (option == null || variationId == null)
            return NotFound("Variation option not found");

        var variation = await variationRepository.GetVariationByIdAsync(variationId);
        if (variation == null)
            return NotFound("Associated variation not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null || user.OwnedRestaurant == null)
            return NotFound("User is not found or do not owned a restaurant");

        var foodItem = await foodItemRepository.GetFoodItemWithCategoryAsync(foodItemId);
        if (foodItem == null)
            return BadRequest("Invalid food item");
        if (foodItem.FoodCategory.RestaurantId != user.OwnedRestaurant.Id)
            return Unauthorized("You do not own this variation option");

        mapper.Map(optionUpdateDto, option);

        if (await variationOptionRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to update variation option");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete("food-categories/{foodCategoryId}/food-items/{foodItemId}/variations/{variationId}/variation-options/{id}")]
    public async Task<ActionResult> DeleteVariationOption(string id, string variationId, string foodItemId, string foodCategoryId)
    {
        var option = await variationOptionRepository.GetVariationOptionByIdAsync(id);
        if (option == null || variationId == null)
            return NotFound("Variation option not found");

        var variation = await variationRepository.GetVariationByIdAsync(variationId);
        if (variation == null)
            return NotFound("Associated variation not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null || user.OwnedRestaurant == null)
            return NotFound("User is not found or do not owned a restaurant");

        var foodItem = await foodItemRepository.GetFoodItemWithCategoryAsync(foodItemId);
        if (foodItem == null)
            return BadRequest("Invalid food item");
        if (foodItem.FoodCategory.RestaurantId != user.OwnedRestaurant.Id)
            return Unauthorized("You do not own this variation option");

        variationOptionRepository.RemoveVariationOption(option);

        if (await variationOptionRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to delete variation option");
    }
}