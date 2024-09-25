using API.DTOs;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UsersController(IUserRepository userRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery]PaginationParams paginationParams)
    {
        var users = await userRepository.GetMembersAsync(paginationParams);

        return Ok(users);
    }
}