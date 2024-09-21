using API.DTOs;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class RestaurantsController(IRestaurantRepository restaurantRepository, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants([FromQuery]RestaurantParams restaurantParams)
    {
        var restaurants = await restaurantRepository.GetRestaurantsAsync(restaurantParams);

        var restaurantsToReturn = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        return Ok(restaurantsToReturn);
    }

}

