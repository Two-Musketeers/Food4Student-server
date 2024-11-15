using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UsersController(IUserRepository userRepository, 
        IMapper mapper, IRoleRepository roleRepository, IShippingAddressRepository shippingAddressRepository) : BaseApiController
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
        // if (await userRepository.GetUserByUsernameAsync(registerDto.Username) != null)
        // {
        //     return BadRequest("Username is taken");
        // }

        var user = new AppUser
        {
            Id = registerDto.Id,
            UserName = registerDto.Username,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            Avatar = registerDto.Avatar
        };

        var role = await roleRepository.GetRoleByNameAsync("User");

        role.Users.Add(user); 

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to register user");

        return Ok("User has successfully registered");
    }

    // [HttpPost("restaurant-register")]
    // public async Task<ActionResult> RegisterRestaurant(RegisterDto registerDto)
    // {
    //     // if (await userRepository.UserExists(registerDto.Id))
    //     // {
    //     //     return BadRequest("How did you even get here ?");
    //     // }

    //     var user = new AppUser
    //     {
    //         Id = registerDto.Id,
    //         UserName = registerDto.Username,
    //         Email = registerDto.Email,
    //         PhoneNumber = registerDto.PhoneNumber,
    //         ShippingAddresses = [mapper.Map<ShippingAddress>(registerDto.Address)],
    //         Avatar = registerDto.Avatar
    //     };

    //     var result = await userRepository.AddUserAsync(user);

    //     if (!result) return BadRequest("Failed to register user");

    //     await userRepository.AddRoleToUserAsync(user.Id, "RestaurantOwner");

    //     return Ok("Restaurant has successfully registered");
    // }

    [HttpPost("{userId}/add-address")]
    public async Task<ActionResult> AddShippingAddress(string userId, ShippingAddressDto shippingAddressDto)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        var shippingAddress = mapper.Map<ShippingAddress>(shippingAddressDto);
        
        user.ShippingAddresses.Add(shippingAddress);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to add shipping address");

        return Ok("Shipping address has been added successfully");
    }

    [HttpPut("update-address")]
    public async Task<ActionResult> UpdateShippingAddress(ShippingAddressDto shippingAddressDto)
    {
        var shippingAddress = await shippingAddressRepository.GetShippingAddressByIdAsync(shippingAddressDto.Id);

        if (shippingAddress == null) return NotFound("Shipping address not found");

        mapper.Map(shippingAddressDto, shippingAddress);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to update shipping address");

        return Ok("Shipping address has been updated successfully");
    }

    [HttpGet("{userId}/shipping-addresses")]
    public async Task<ActionResult<IEnumerable<ShippingAddressDto>>> GetShippingAddresses(string userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        var shippingAddresses = user.ShippingAddresses;

        return Ok(mapper.Map<IEnumerable<ShippingAddressDto>>(shippingAddresses));
    }
    [HttpDelete("{shippingAddressId}/delete-address")]
    public async Task<ActionResult> DeleteShippingAddress(int shippingAddressId)
    {
        var shippingAddress = await shippingAddressRepository.GetShippingAddressByIdAsync(shippingAddressId);

        if (shippingAddress == null) return NotFound("Shipping address not found");

        shippingAddressRepository.DeleteShippingAddress(shippingAddress);

        var result = await userRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to delete shipping address");

        return Ok("Shipping address has been deleted successfully");
    }
}