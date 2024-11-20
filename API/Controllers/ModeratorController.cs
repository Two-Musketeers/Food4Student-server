using API.DTOs;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "RequireModeratorRole")]
public class ModeratorController(IMapper mapper, IFirebaseService firebaseService,
    IRestaurantRepository restaurantRepository) : BaseApiController
{
    //Get all unapproved restaurants

    [HttpGet("unapproved-restaurant")]
    public async Task<ActionResult<IEnumerable<GeneralRestaurantDto>>> GetUnapprovedRestaurants([FromQuery] RestaurantParams restaurantParams)
    {
        var restaurants = await restaurantRepository.UnapprovedRestaurantsAsync(restaurantParams);

        var restaurantsToReturn = mapper.Map<IEnumerable<GeneralRestaurantDto>>(restaurants);

        return Ok(restaurantsToReturn);
    }

    [HttpPut("ban-user")]
    public async Task<ActionResult> BanUser(string id)
    {
        var userRole = await firebaseService.GetUserRoleAsync(id);
        if (userRole == "Admin") return Unauthorized("You do not have permission to ban me bitch");
        await firebaseService.AssignRoleAsync(id, "Banned");
        return Ok();
    }

    [HttpPut("unban-user")]
    public async Task<ActionResult> UnbanUser(string id)
    {
        var userRole = await firebaseService.GetUserRoleAsync(id);
        if (userRole == "Admin") return Unauthorized("Don't test the water mate");
        await firebaseService.AssignRoleAsync(id, "User");
        return Ok();
    }

    [HttpPut("unban-restaurant-owner")]
    public async Task<ActionResult> UnbanRestaurantOwner(string id)
    {
        var userRole = await firebaseService.GetUserRoleAsync(id);
        if (userRole == "Admin") return Unauthorized("You think i didn't think of this ? Do better");
        await firebaseService.AssignRoleAsync(id, "RestaurantOwner");
        return Ok();
    }

    [HttpPut("approve-restaurant")]
    public async Task<ActionResult> ApproveRestaurant(string id)
    {
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(id);

        if (restaurant == null) return NotFound();

        restaurant.IsApproved = true;

        restaurantRepository.Update(restaurant);

        if (await restaurantRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to approve restaurant");
    }

    [HttpPut("unapprove-restaurant")]
    public async Task<ActionResult> UnApprovedRestaurant(string id)
    {
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(id);

        if (restaurant == null) return NotFound();

        restaurant.IsApproved = false;

        restaurantRepository.Update(restaurant);

        if (await restaurantRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to unapprove restaurant");
    }
}

