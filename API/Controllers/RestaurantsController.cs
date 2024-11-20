using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class RestaurantsController(IRestaurantRepository restaurantRepository,
        IMapper mapper, IUserRepository userRepository) : BaseApiController
{
    //Get all restaurant details
    [Authorize(Policy = "RequireUserRole")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants([FromQuery] RestaurantParams restaurantParams)
    {
        var restaurants = await restaurantRepository.GetRestaurantsAsync(restaurantParams);

        var restaurantsToReturn = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        return Ok(restaurantsToReturn);
    }

    //Get restaurant basic info only like name, logo, location, rating
    [HttpGet("general")]
    public async Task<ActionResult<IEnumerable<GeneralRestaurantDto>>> GetGeneralRestaurants([FromQuery] RestaurantParams restaurantParams)
    {
        var restaurants = await restaurantRepository.GetRestaurantsAsync(restaurantParams);

        var restaurantsToReturn = mapper.Map<IEnumerable<GeneralRestaurantDto>>(restaurants);

        return Ok(restaurantsToReturn);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RestaurantDto>> GetRestaurantById(string id)
    {
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(id);

        if (restaurant == null) return NotFound();

        var restaurantToReturn = mapper.Map<RestaurantDto>(restaurant);

        return Ok(restaurantToReturn);
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpPost]
    public async Task<ActionResult<RestaurantDto>> AddRestaurant(RestaurantRegisterDto restaurantRegisterDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant != null) return BadRequest("User already owns a restaurant");

        var restaurant = new Restaurant
        {
            Id = userId,
            Name = restaurantRegisterDto.Name,
            Description = restaurantRegisterDto.Description,
            Address = restaurantRegisterDto.Address,
            Latitude = restaurantRegisterDto.Latitude,
            Longitude = restaurantRegisterDto.Longitude,
        };

        user.OwnedRestaurant = restaurant;

        await restaurantRepository.SaveAllAsync();

        return Ok(mapper.Map<RestaurantDto>(restaurant));
    }


    [HttpPut]
    public async Task<ActionResult<RestaurantDto>> UpdateRestaurant(RestaurantRegisterDto restaurantUpdateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        var restaurant = user.OwnedRestaurant;

        restaurant.Name = restaurantUpdateDto.Name;
        restaurant.Description = restaurantUpdateDto.Description;
        restaurant.Address = restaurantUpdateDto.Address;
        restaurant.Latitude = restaurantUpdateDto.Latitude;
        restaurant.Longitude = restaurantUpdateDto.Longitude;

        await restaurantRepository.SaveAllAsync();

        return Ok(mapper.Map<RestaurantDto>(restaurant));
    }

    [Authorize(Policy = "RequireRestaurantOwnerRole")]
    [HttpDelete]
    public async Task<ActionResult> DeleteRestaurant()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var user = await userRepository.GetUserByIdAsync(userId);

        if (user.OwnedRestaurant == null) return BadRequest("User does not own a restaurant");

        restaurantRepository.DeleteRestaurant(user.OwnedRestaurant);

        await restaurantRepository.SaveAllAsync();

        return NoContent();
    }
}

