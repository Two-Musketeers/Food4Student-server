using API.DTOs;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "RequireAdminRole")]
public class AdminController(IRestaurantRepository restaurantRepository,
    IMapper mapper, IUserRepository userRepository, IFirebaseService firebaseService) : BaseApiController
{
    [HttpPut("give-moderator-role")]
    public async Task<ActionResult> GiveModeratorRole(string id)
    {
        var user = await userRepository.GetUserByIdAsync(id);
        if (user == null) return NotFound();
        await firebaseService.AssignRoleAsync(id, "Moderator");
        return Ok();
    }

    [HttpPut("ban-user")]
    public async Task<ActionResult> BanUser(string id)
    {
        var userRole = await firebaseService.GetUserRoleAsync(id);
        if (userRole == "Admin") return Unauthorized("You do not have permission to ban me bitch");
        await firebaseService.AssignRoleAsync(id, "Banned");
        return Ok();
    }
}

