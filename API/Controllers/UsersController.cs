using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UsersController(IUserRepository userRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] PaginationParams paginationParams)
    {
        var users = await userRepository.GetMembersAsync(paginationParams);

        return Ok(users);
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        // if (await userRepository.UserExists(registerDto.Id))
        // {
        //     return BadRequest("How did you even get here ?");
        // }

        var user = new AppUser
        {
            Id = registerDto.Id,
            UserName = registerDto.Username,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            Address = registerDto.Address,
            Latitude = registerDto.Latitude,
            Longitude = registerDto.Longitude,
            Avatar = registerDto.Avatar
        };

        var result = await userRepository.AddUserAsync(user);

        if (!result) return BadRequest("Failed to register user");

        await userRepository.AddRoleToUserAsync(user.Id, "User");

        return Ok("User has successfully registered");
    }
}