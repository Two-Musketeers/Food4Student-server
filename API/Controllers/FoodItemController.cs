using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class FoodItemController(IFoodItemRepository foodItemRepository,
        IUserRepository userRepository, IMapper mapper) : BaseApiController
{
    [HttpPost]
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

    [HttpPut("update-food-item")]
    public async Task<ActionResult> UpdateFoodItem(FoodItemDto foodItemDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (foodItemDto.Id == null) return NotFound("Food item not found");

        var foodItem = await foodItemRepository.GetFoodItemByIdAsync(foodItemDto.Id);

        // Ensure the food item belongs to the authenticated user
        if (foodItem.RestaurantId != userId) return Forbid();
        
        mapper.Map(foodItemDto, foodItem);

        var result = await foodItemRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to update food item");

        return Ok("Food item has been updated successfully");
    }

    //The get restaurant by id already contain the menu so i just make this in case
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetFoodItems()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        var foodItems = await foodItemRepository.GetFoodItemsByRestaurantIdAsync(user.Id);

        var foodItemsToReturn = mapper.Map<IEnumerable<FoodItemDto>>(foodItems);

        return Ok(foodItemsToReturn);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFoodItem(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
}
