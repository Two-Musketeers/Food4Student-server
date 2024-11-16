using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

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
        if (await userRepository.UserExists(registerDto.Id)) return BadRequest("How did you even get here ?");

        var user = new AppUser
        {
            Id = registerDto.Id,
            UserName = registerDto.Username,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            Avatar = registerDto.Avatar
        };

        userRepository.AddUser(user);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to register restaurant owner");

        await firebaseService.AssignRoleAsync(registerDto.Id, "RestaurantOwner");

        return Ok("Restaurant Owner has successfully registered");
    }

}