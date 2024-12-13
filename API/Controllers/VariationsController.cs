using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/restaurants/[controller]")]
public class VariationsController(
    IVariationOptionRepository variationOptionRepository,
    IVariationRepository variationRepository,
    IUserRepository userRepository,
    IMapper mapper,
    IFoodItemVariationRepository foodItemVariationRepository,
    IFoodItemRepository foodItemRepository) : ControllerBase
{
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VariationDto>>> GetVariationsByRestaurant()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null || user.OwnedRestaurant.Id == null)
            return BadRequest("User does not own a restaurant");

        var variations = await variationRepository.GetVariationsByRestaurantIdAsync(user.OwnedRestaurant.Id);

        var variationDtos = mapper.Map<IEnumerable<VariationDto>>(variations);
        return Ok(variationDtos);
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost]
    public async Task<ActionResult<VariationDto>> CreateVariation(VariationCreateDto variationCreateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        var variation = mapper.Map<Variation>(variationCreateDto);
        variation.RestaurantId = user.OwnedRestaurant.Id;

        variationRepository.AddVariation(variation);

        if (await variationRepository.SaveAllAsync())
        {
            var variationToReturn = mapper.Map<VariationDto>(variation);
            return CreatedAtAction(nameof(GetVariationById), new { id = variation.Id }, variationToReturn);
        }

        return BadRequest("Failed to create variation");
    }

    // Don't know if we need this
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpGet("{id}")]
    public async Task<ActionResult<VariationDto>> GetVariationById(string id)
    {
        var variation = await variationRepository.GetVariationByIdAsync(id);
        if (variation == null)
            return NotFound("Variation not found");

        var variationDto = mapper.Map<VariationDto>(variation);
        return Ok(variationDto);
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateVariation(string id, VariationCreateDto variationUpdateDto)
    {
        var variation = await variationRepository.GetVariationByIdAsync(id);
        if (variation == null)
            return NotFound("Variation not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user == null || user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        if (variation.RestaurantId != user.OwnedRestaurant.Id)
            return Unauthorized("You do not own this variation");

        mapper.Map(variationUpdateDto, variation);

        if (await variationRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to update variation");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteVariation(string id)
    {
        var variation = await variationRepository.GetVariationByIdAsync(id);
        if (variation == null)
            return NotFound("Variation not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user == null || user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        if (variation.RestaurantId != user.OwnedRestaurant.Id)
            return Unauthorized("You do not own this variation");

        variationRepository.DeleteVariation(variation);

        if (await variationRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to delete variation");
    }

    // VariationOption Controller
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("{variationId}/variation-options")]
    public async Task<ActionResult<VariationOptionDto>> CreateVariationOption(string variationId, VariationOptionCreateDto optionCreateDto)
    {
        var variation = await variationRepository.GetVariationByIdAsync(variationId);
        if (variation == null)
            return NotFound("Variation not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user == null || user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        if (variation.RestaurantId != user.OwnedRestaurant.Id)
            return Unauthorized("You do not own this variation");

        var variationOption = mapper.Map<VariationOption>(optionCreateDto);
        variationOption.VariationId = variationId;

        variationOptionRepository.AddVariationOption(variationOption);

        if (await variationOptionRepository.SaveAllAsync())
        {
            var optionToReturn = mapper.Map<VariationOptionDto>(variationOption);
            return CreatedAtAction(nameof(GetVariationOptionById), new { id = variationOption.Id }, optionToReturn);
        }

        return BadRequest("Failed to create variation option");
    }

    // Don't know if we will be needing this
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpGet("variation-options/{id}")]
    public async Task<ActionResult<VariationOptionDto>> GetVariationOptionById(string id)
    {
        var option = await variationOptionRepository.GetVariationOptionByIdAsync(id);
        if (option == null)
            return NotFound("Variation option not found");

        var optionDto = mapper.Map<VariationOptionDto>(option);
        return Ok(optionDto);
    }


    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPut("{variationId}/variation-options/{id}")]
    public async Task<ActionResult<VariationOptionDto>> UpdateVariationOption(string id, string variationId, VariationOptionCreateDto optionUpdateDto)
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
            return BadRequest("User does not own a restaurant");

        if (variation.RestaurantId != user.OwnedRestaurant.Id)
            return Unauthorized("You do not own this variation option");

        var returnVariationOption = mapper.Map(optionUpdateDto, option);

        if (await variationOptionRepository.SaveAllAsync())
            return Ok(returnVariationOption);

        return BadRequest("Failed to update variation option");
    }
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete("{variationId}/variation-options/{id}")]
    public async Task<ActionResult> DeleteVariationOption(string id, string variationId)
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
        if (variation.RestaurantId != user.OwnedRestaurant.Id)
            return Unauthorized("You do not own this variation option");

        variationOptionRepository.DeleteVariationOption(option);

        if (await variationOptionRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to delete variation option");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("{variationId}/variation-options/{variationOptionId}/food-items/{foodItemId}")]
    public async Task<ActionResult<FoodItemVariationDto>> AddVariationToFoodItem(string variationId, string variationOptionId, string foodItemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        if (foodItemId == null || variationId == null || variationOptionId == null)
            return BadRequest("Invalid request");

        // Fetch FoodItem with FoodCategory
        var foodItem = await foodItemRepository.GetFoodItemWithCategoryAsync(foodItemId);
        if (foodItem == null)
            return BadRequest("Invalid food item");

        var restaurantId = foodItem.FoodCategory?.RestaurantId;
        if (restaurantId == null || restaurantId != user.OwnedRestaurant.Id)
            return BadRequest("Invalid food item or you do not own the restaurant");

        var variation = await variationRepository.GetVariationByIdAsync(variationId);
        if (variation == null || variation.RestaurantId != user.OwnedRestaurant.Id)
            return BadRequest("Invalid variation");

        var variationOption = await variationOptionRepository.GetVariationOptionByIdAsync(variationOptionId);
        if (variationOption == null || variationOption.VariationId != variationId)
            return BadRequest("Invalid variation option");

        var existingFiv = await foodItemVariationRepository.GetFoodItemVariationAsync(
            foodItemId, variationId, variationOptionId);
        if (existingFiv != null)
            return BadRequest("Variation option already associated with this food item");

        var foodItemVariation = new FoodItemVariation
        {
            FoodItemId = foodItemId,
            VariationId = variationId,
            VariationOptionId = variationOptionId
        };

        foodItemVariationRepository.AddFoodItemVariation(foodItemVariation);

        if (await foodItemVariationRepository.SaveAllAsync())
            return Ok(foodItemVariation);

        return BadRequest("Failed to add variation option to food item");
    }

    [HttpGet("{id}/variation-options/{optionId}/food-items/{foodItemId}")]
    public async Task<ActionResult<IEnumerable<FoodItemVariationDto>>> GetVariationsForFoodItem(string foodItemId, string id, string optionId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        // Fetch FoodItem with FoodCategory
        var foodItem = await foodItemRepository.GetFoodItemWithCategoryAsync(foodItemId);
        if (foodItem == null)
            return BadRequest("Invalid food item");

        var restaurantId = foodItem.FoodCategory?.RestaurantId;
        if (restaurantId == null || restaurantId != user.OwnedRestaurant.Id)
            return BadRequest("Invalid food item or you do not own the restaurant");

        var variations = await foodItemVariationRepository.GetVariationsByFoodItemIdAsync(foodItemId);

        var variationDtos = mapper.Map<IEnumerable<FoodItemVariationDto>>(variations);
        return Ok(variationDtos);
    }

    [HttpDelete("{variationId}/variation-options/{variationOptionId}/food-items/{foodItemId}")]
    public async Task<ActionResult> RemoveVariationFromFoodItem(string foodItemId, string variationId, string variationOptionId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        if (foodItemId == null || variationId == null || variationOptionId == null)
            return BadRequest("Invalid request");

        var fiv = await foodItemVariationRepository.GetFoodItemVariationAsync(
            foodItemId, variationId, variationOptionId);
        if (fiv == null)
            return NotFound("Variation option association not found");

        // Fetch FoodItem with FoodCategory to verify ownership
        var foodItem = await foodItemRepository.GetFoodItemWithCategoryAsync(foodItemId);
        if (foodItem == null)
            return BadRequest("Invalid food item");

        var restaurantId = foodItem.FoodCategory?.RestaurantId;
        if (restaurantId == null || restaurantId != user.OwnedRestaurant.Id)
            return BadRequest("You do not own this food item's restaurant");

        foodItemVariationRepository.DeleteFoodItemVariation(fiv);

        if (await foodItemVariationRepository.SaveAllAsync())
            return Ok("Variation option removed from food item");

        return BadRequest("Failed to remove variation option from food item");
    }
}