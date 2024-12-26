using API.DTOs;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "RequireModeratorRole")]
public class ModeratorController(IMapper mapper, IFirebaseService firebaseService,
    IRestaurantRepository restaurantRepository, IUserRepository userRepository) : BaseApiController
{
    //Get all unapproved restaurants
    [HttpGet("restaurants")]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants([FromQuery] PaginationParams restaurantParams)
    {
        var restaurants = await restaurantRepository.GetRestaurantForAdmin(restaurantParams);

        var restaurantsToReturn = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

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

    [HttpPut("restaurants/approve/{id}")]
    public async Task<ActionResult<RestaurantDto>> ApproveRestaurant(string id)
    {
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(id);

        if (restaurant == null) return NotFound();

        restaurant.IsApproved = true;

        restaurantRepository.Update(restaurant);

        var returnRestaurant = mapper.Map<RestaurantDto>(restaurant);

        if (await restaurantRepository.SaveAllAsync()) return returnRestaurant;
        
        return BadRequest("Failed to approve restaurant");
    }

    [HttpPut("restaurants/unapprove/{id}")]
    public async Task<ActionResult<RestaurantDto>> UnApprovedRestaurant(string id)
    {
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(id);

        if (restaurant == null) return NotFound();

        restaurant.IsApproved = false;

        restaurantRepository.Update(restaurant);

        var returnRestaurant = mapper.Map<RestaurantDto>(restaurant);

        if (await restaurantRepository.SaveAllAsync()) return returnRestaurant;

        return BadRequest("Failed to unapprove restaurant");
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] PaginationParams paginationParams)
    {
        var users = await userRepository.GetMembersAsync(paginationParams);

        var usersWithDetails = new List<UserDto>();

        foreach (var user in users)
        {
            var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(user.Id);

            var userDto = new UserDto
            {
                Id = user.Id,
                PhoneNumber = userRecord.PhoneNumber,
                DisplayName = userRecord.DisplayName,
                Email = userRecord.Email,
                Role = userRecord.CustomClaims.ContainsKey("role") ? userRecord.CustomClaims["role"].ToString() : "User",
                OwnedRestaurant = user.OwnedRestaurant != null
            };

            usersWithDetails.Add(userDto);
        }

        return Ok(usersWithDetails);
    }
}

