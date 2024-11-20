using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
/// <summary>
/// Controller for register and delete user account
/// </summary>
public class AccountController (IUserRepository userRepository,
    IFirebaseService firebaseService) : BaseApiController
{
    [HttpPost("user")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        if (await userRepository.UserExists(userId)) return BadRequest("How did you even get here ?");

        var user = new AppUser
        {
            Id = userId,
            DisplayName = registerDto.Username,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
        };

        userRepository.AddUser(user);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to register user");

        await firebaseService.AssignRoleAsync(userId, "User");

        return Ok("User has successfully registered");
    }

    [HttpPost("restaurant")]
    public async Task<ActionResult> RegisterRestaurant(RegisterDto registerDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        if (await userRepository.UserExists(userId)) return BadRequest("How did you even get here ?");

        var user = new AppUser
        {
            Id = userId,
            DisplayName = registerDto.Username,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
        };

        userRepository.AddUser(user);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to register restaurant owner");

        await firebaseService.AssignRoleAsync(userId, "RestaurantOwner");

        return Ok("Restaurant Owner has successfully registered");
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user == null) return NotFound();

        userRepository.RemoveUser(user);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to delete user");

        return Ok("User has been deleted");
    }

    #if DEBUG
    [HttpPost("admin-register")]
    public async Task<ActionResult> RegisterAdmin(RegisterDto registerDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        if (await userRepository.UserExists(userId)) return BadRequest("How did you even get here ?");

        var user = new AppUser
        {
            Id = userId,
            DisplayName = registerDto.Username,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
        };

        userRepository.AddUser(user);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to register admin");

        await firebaseService.AssignRoleAsync(userId, "Admin");

        return Ok("Admin has successfully registered");
    }

    //Development purpose only
    //TODO: Delete this when done
    [HttpPost("moderator-register")]
    public async Task<ActionResult> RegisterModerator(RegisterDto registerDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        if (await userRepository.UserExists(userId)) return BadRequest("How did you even get here ?");

        var user = new AppUser
        {
            Id = userId,
            DisplayName = registerDto.Username,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
        };

        userRepository.AddUser(user);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to register moderator");

        await firebaseService.AssignRoleAsync(userId, "Moderator");

        return Ok("Moderator has successfully registered");
    }
    #endif
}

