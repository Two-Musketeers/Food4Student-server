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
    IFoodItemRepository foodItemRepository) : ControllerBase
{
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("food-items/{foodItemId}/variations")]
    public async Task<ActionResult<VariationDto>> CreateVariation(VariationCreateDto variationCreateDto, string foodItemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        var foodItem = await foodItemRepository.GetFoodItemWithCategoryAsync(foodItemId);

        if (foodItem == null)
            return BadRequest("Invalid food item");

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        var variation = mapper.Map<Variation>(variationCreateDto);
        variation.FoodItemId = foodItemId;
        
        foodItem.Variations.Add(variation);

        variationRepository.AddVariation(variation);

        if (await variationRepository.SaveAllAsync())
        {
            var variationToReturn = mapper.Map<VariationDto>(variation);
            return Ok(variationToReturn);
        }

        return BadRequest("Failed to create variation");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPut("food-items/{foodItemId}/variations/{id}")]
    public async Task<ActionResult> UpdateVariation(string id, VariationCreateDto variationUpdateDto, string foodItemId)
    {
        var foodItem = await foodItemRepository.GetFoodItemWithCategoryAsync(foodItemId);
        if (foodItem == null)
            return BadRequest("Invalid food item");
        var variation = await variationRepository.GetVariationByIdAsync(id);
        if (variation == null)
            return NotFound("Variation not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (variation.FoodItemId != foodItemId)
            return BadRequest("Variation does not belong to this food item");

        if (foodItem.FoodCategory?.RestaurantId != user.OwnedRestaurant?.Id)
            return Unauthorized("You do not own this variation");

        mapper.Map(variationUpdateDto, variation);

        if (await variationRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to update variation");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete("food-items/{foodItemId}/variations/{id}")]
    public async Task<ActionResult> DeleteVariation(string id, string foodItemId)
    {
        var variation = await variationRepository.GetVariationByIdAsync(id);
        if (variation == null)
            return NotFound("Variation not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user == null || user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        if (variation.FoodItemId != foodItemId)
            return BadRequest("Variation does not belong to this food item");

        if (user.OwnedRestaurant.Id != variation.FoodItem.FoodCategory?.RestaurantId)
            return Unauthorized("You do not own this variation");

        variationRepository.RemoveVariation(variation);

        if (await variationRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to delete variation");
    }
    
    // VariationOption Controller
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("food-items/{foodItemId}/variations/{variationId}/variation-options")]
    public async Task<ActionResult<VariationOptionDto>> CreateVariationOption(string variationId, VariationOptionCreateDto optionCreateDto, string foodItemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        var foodItem = await foodItemRepository.GetFoodItemWithCategoryAsync(foodItemId);
        if (foodItem == null)
            return BadRequest("Invalid food item");

        var variation = await variationRepository.GetVariationByIdAsync(variationId);
        if (variation == null)
            return BadRequest("Invalid variation");

        if (variation.FoodItemId != foodItemId)
            return BadRequest("Variation does not belong to this food item");

        if (foodItem.FoodCategory.RestaurantId != user.OwnedRestaurant.Id)
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
    [HttpPut("food-items/{foodItemId}/variations/{variationId}/variation-options/{id}")]
    public async Task<ActionResult<VariationOptionDto>> UpdateVariationOption(string id, string variationId, VariationOptionCreateDto optionUpdateDto, string foodItemId)
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
    [HttpDelete("food-items/{foodItemId}/variations/{variationId}/variation-options/{id}")]
    public async Task<ActionResult> DeleteVariationOption(string id, string variationId, string foodItemId)
    {
        var option = await variationOptionRepository.GetVariationOptionByIdAsync(id);
        if (option == null)
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