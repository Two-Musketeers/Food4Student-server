using API.DTOs;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "RequireAdminRole")]
public class AdminController(IUserRepository userRepository, IFirebaseService firebaseService) : BaseApiController
{
    [HttpPut("give-moderator-role")]
    public async Task<ActionResult> GiveModeratorRole(string id)
    {
        var user = await userRepository.GetUserByIdAsync(id);
        if (user == null) return NotFound();
        await firebaseService.AssignRoleAsync(id, "Moderator");
        return Ok();
    }

    [HttpPut("revoke-moderator-role")]
    public async Task<ActionResult> RevokeModeratorRole(string id)
    {
        var userRole = await firebaseService.GetUserRoleAsync(id);
        if (userRole == "Admin") return Unauthorized("You do not have permission to revoke admin roles.");
        await firebaseService.AssignRoleAsync(id, "User");
        return Ok();
    }
}

