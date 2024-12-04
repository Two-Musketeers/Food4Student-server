using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class VariationsController(
    IVariationOptionRepository variationOptionRepository,
    IVariationRepository variationRepository,
    IUserRepository userRepository,
    IMapper mapper,
    IFoodItemVariationRepository foodItemVariationRepository,
    IFoodItemRepository foodItemRepository) : BaseApiController
{
    // Variation Controller
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("variations")]
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

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpGet("variations/{id}")]
    public async Task<ActionResult<VariationDto>> GetVariationById(string id)
    {
        var variation = await variationRepository.GetVariationByIdAsync(id);
        if (variation == null)
            return NotFound("Variation not found");

        var variationDto = mapper.Map<VariationDto>(variation);
        return Ok(variationDto);
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPut("variations/{id}")]
    public async Task<ActionResult> UpdateVariation(string id, VariationCreateDto variationUpdateDto)
    {
        var variation = await variationRepository.GetVariationByIdAsync(id);
        if (variation == null)
            return NotFound("Variation not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (variation.RestaurantId != user.OwnedRestaurant.Id)
            return Unauthorized("You do not own this variation");

        mapper.Map(variationUpdateDto, variation);

        if (await variationRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to update variation");
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete("variations/{id}")]
    public async Task<ActionResult> DeleteVariation(string id)
    {
        var variation = await variationRepository.GetVariationByIdAsync(id);
        if (variation == null)
            return NotFound("Variation not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (variation.RestaurantId != user.OwnedRestaurant.Id)
            return Unauthorized("You do not own this variation");

        variationRepository.DeleteVariation(variation);

        if (await variationRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to delete variation");
    }

    // VariationOption Controller
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("variations/{variationId}/options")]
    public async Task<ActionResult<VariationOptionDto>> CreateVariationOption(string variationId, VariationOptionCreateDto optionCreateDto)
    {
        var variation = await variationRepository.GetVariationByIdAsync(variationId);
        if (variation == null)
            return NotFound("Variation not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

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
    [HttpPut("variation-options/{id}")]
    public async Task<ActionResult> UpdateVariationOption(string id, VariationOptionCreateDto optionUpdateDto)
    {
        var option = await variationOptionRepository.GetVariationOptionByIdAsync(id);
        if (option == null)
            return NotFound("Variation option not found");

        var variation = await variationRepository.GetVariationByIdAsync(option.VariationId);
        if (variation == null)
            return NotFound("Associated variation not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (variation.RestaurantId != user.OwnedRestaurant.Id)
            return Unauthorized("You do not own this variation option");

        mapper.Map(optionUpdateDto, option);

        if (await variationOptionRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to update variation option");
    }
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete("variation-options/{id}")]
    public async Task<ActionResult> DeleteVariationOption(string id)
    {
        var option = await variationOptionRepository.GetVariationOptionByIdAsync(id);
        if (option == null)
            return NotFound("Variation option not found");

        var variation = await variationRepository.GetVariationByIdAsync(option.VariationId);
        if (variation == null)
            return NotFound("Associated variation not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (variation.RestaurantId != user.OwnedRestaurant.Id)
            return Unauthorized("You do not own this variation option");

        variationOptionRepository.DeleteVariationOption(option);

        if (await variationOptionRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to delete variation option");
    }
    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost("food-items/{foodItemId}/variations")]
    public async Task<ActionResult> AddVariationToFoodItem(string foodItemId, FoodItemVariationCreateDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        var foodItem = await foodItemRepository.GetFoodItemByIdAsync(dto.FoodItemId);
        if (foodItem == null || foodItem.RestaurantId != user.OwnedRestaurant.Id)
            return BadRequest("Invalid food item");

        var variation = await variationRepository.GetVariationByIdAsync(dto.VariationId);
        if (variation == null || variation.RestaurantId != user.OwnedRestaurant.Id)
            return BadRequest("Invalid variation");

        var variationOption = await variationOptionRepository.GetVariationOptionByIdAsync(dto.VariationOptionId);
        if (variationOption == null || variationOption.VariationId != dto.VariationId)
            return BadRequest("Invalid variation option");

        var existingFiv = await foodItemVariationRepository.GetFoodItemVariationAsync(
            dto.FoodItemId, dto.VariationId, dto.VariationOptionId);
        if (existingFiv != null)
            return BadRequest("Variation option already associated with this food item");

        var foodItemVariation = new FoodItemVariation
        {
            FoodItemId = dto.FoodItemId,
            VariationId = dto.VariationId,
            VariationOptionId = dto.VariationOptionId
        };

        foodItemVariationRepository.AddFoodItemVariation(foodItemVariation);

        if (await foodItemVariationRepository.SaveAllAsync())
            return Ok("Variation option added to food item");

        return BadRequest("Failed to add variation option to food item");
    }
    [HttpGet("food-items/{foodItemId}")]
    public async Task<ActionResult<IEnumerable<FoodItemVariationDto>>> GetVariationsForFoodItem(string foodItemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        var foodItem = await foodItemRepository.GetFoodItemByIdAsync(foodItemId);
        if (foodItem == null || foodItem.RestaurantId != user.OwnedRestaurant.Id)
            return BadRequest("Invalid food item");

        var variations = await foodItemVariationRepository.GetVariationsByFoodItemIdAsync(foodItemId);

        var variationDtos = mapper.Map<IEnumerable<FoodItemVariationDto>>(variations);
        return Ok(variationDtos);
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveVariationFromFoodItem(FoodItemVariationCreateDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null)
            return BadRequest("User does not own a restaurant");

        var fiv = await foodItemVariationRepository.GetFoodItemVariationAsync(
            dto.FoodItemId, dto.VariationId, dto.VariationOptionId);
        if (fiv == null)
            return NotFound("Variation option association not found");

        foodItemVariationRepository.DeleteFoodItemVariation(fiv);

        if (await foodItemVariationRepository.SaveAllAsync())
            return Ok("Variation option removed from food item");

        return BadRequest("Failed to remove variation option from food item");
    }
}

