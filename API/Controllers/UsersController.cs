using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IFirebaseService firebaseService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] PaginationParams paginationParams)
    {
        var users = await userRepository.GetMembersAsync(paginationParams);

        return Ok(users);
    }
    
    [HttpPost("user-register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (await userRepository.UserExists(userId)) return BadRequest("How did you even get here ?");

        var user = new AppUser
        {
            Id = userId,
            UserName = registerDto.Username,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            Avatar = registerDto.Avatar
        };

        userRepository.AddUser(user);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to register user");

        await firebaseService.AssignRoleAsync(userId, "User");

        return Ok("User has successfully registered");
    }

    [HttpPost("restaurant-register")]
    public async Task<ActionResult> RegisterRestaurant(RegisterDto registerDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (await userRepository.UserExists(userId)) return BadRequest("How did you even get here ?");

        var user = new AppUser
        {
            Id = userId,
            UserName = registerDto.Username,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            Avatar = registerDto.Avatar
        };

        userRepository.AddUser(user);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to register restaurant owner");

        await firebaseService.AssignRoleAsync(userId, "RestaurantOwner");

        return Ok("Restaurant Owner has successfully registered");
    }

    //Development purpose only
    //TODO: Delete this when done
    [HttpPost("admin-register")]
    public async Task<ActionResult> RegisterAdmin(RegisterDto registerDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (await userRepository.UserExists(userId)) return BadRequest("How did you even get here ?");

        var user = new AppUser
        {
            Id = userId,
            UserName = registerDto.Username,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            Avatar = registerDto.Avatar
        };

        userRepository.AddUser(user);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to register admin");

        await firebaseService.AssignRoleAsync(userId, "Admin");

        return Ok("Admin has successfully registered");
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user == null) return NotFound();

        userRepository.RemoveUser(user);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to delete user");

        return Ok("User has been deleted");
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(UserDto userDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user == null) return NotFound();

        user.UserName = userDto.UserName;
        user.Email = userDto.Email;
        user.PhoneNumber = userDto.PhoneNumber;
        user.Avatar = userDto.Avatar;

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to update user");

        return Ok("User has been updated");
    }
}