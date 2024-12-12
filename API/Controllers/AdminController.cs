using System.Security.Claims;
using API.DTOs;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "RequireAdminRole")]
public class AdminController(IFirebaseService firebaseService) : BaseApiController
{
    [HttpPut("give-moderator-role")]
    public async Task<ActionResult> GiveModeratorRole(string id)
    {
        await firebaseService.AssignRoleAsync(id, "Moderator");
        return Ok();
    }

    [HttpPut("revoke-moderator-role")]
    public async Task<ActionResult> RevokeModeratorRole(string id)
    {
        await firebaseService.AssignRoleAsync(id, "User");
        return Ok();
    }
}

