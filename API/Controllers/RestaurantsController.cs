using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class RestaurantsController(IRestaurantRepository restaurantRepository, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants([FromQuery] RestaurantParams restaurantParams)
    {
        var restaurants = await restaurantRepository.GetRestaurantsAsync(restaurantParams);

        var restaurantsToReturn = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        return Ok(restaurantsToReturn);
    }
    [HttpGet("general")]
    public async Task<ActionResult<IEnumerable<GeneralRestaurantDto>>> GetGeneralRestaurants([FromQuery] RestaurantParams restaurantParams)
    {
        var restaurants = await restaurantRepository.GetGeneralRestaurantsAsync(restaurantParams);

        return Ok(restaurants);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<RestaurantDto>> GetRestaurantById(string id)
    {
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(id);

        if (restaurant == null) return NotFound();

        return Ok(restaurant);
    }
    [HttpPost]
    public async Task<ActionResult<RestaurantDto>> AddRestaurant(RestaurantDto restaurantDto)
    {
        var restaurant = mapper.Map<Restaurant>(restaurantDto);

        restaurantRepository.AddRestaurant(restaurant);

        return CreatedAtAction(nameof(GetRestaurantById), new { id = restaurant.Id }, restaurant);
    }
}

