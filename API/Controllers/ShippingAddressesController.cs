using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class ShippingAddressesController(IShippingAddressRepository shippingAddressRepository,
    IUserRepository userRepository, IMapper mapper) : BaseApiController
{
    [HttpPost("add-address")]
    public async Task<ActionResult> AddShippingAddress(ShippingAddressDto shippingAddressDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        var shippingAddress = mapper.Map<ShippingAddress>(shippingAddressDto);
        
        user.ShippingAddresses.Add(shippingAddress);

        var result = await shippingAddressRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to add shipping address");

        return Ok("Shipping address has been added successfully");
    }

    [HttpPut("update-address/{addressId}")]
    public async Task<ActionResult> UpdateShippingAddress(int addressId, ShippingAddressDto shippingAddressDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var shippingAddress = await shippingAddressRepository.GetShippingAddressByIdAsync(addressId);
        if (shippingAddress == null) return NotFound("Shipping address not found");

        // Ensure the shipping address belongs to the authenticated user
        if (shippingAddress.AppUserId != userId) return Forbid();

        shippingAddressDto.Id = addressId;
        
        mapper.Map(shippingAddressDto, shippingAddress);

        var result = await shippingAddressRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to update shipping address");

        return Ok("Shipping address has been updated successfully");
    }

    [HttpGet("shipping-addresses")]
    public async Task<ActionResult<IEnumerable<ShippingAddressDto>>> GetShippingAddresses()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        var shippingAddresses = user.ShippingAddresses;

        return Ok(mapper.Map<IEnumerable<ShippingAddressDto>>(shippingAddresses));
    }

    [HttpDelete("{shippingAddressId}/delete-address")]
    public async Task<ActionResult> DeleteShippingAddress(int shippingAddressId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var shippingAddress = await shippingAddressRepository.GetShippingAddressByIdAsync(shippingAddressId);

        if (shippingAddress == null) return NotFound("Shipping address not found");

        if (shippingAddress.AppUserId != userId) return Forbid();

        shippingAddressRepository.DeleteShippingAddress(shippingAddress);

        var result = await shippingAddressRepository.SaveAllAsync();

        if (!result) return BadRequest("Failed to delete shipping address");

        return Ok("Shipping address has been deleted successfully");
    }
}

