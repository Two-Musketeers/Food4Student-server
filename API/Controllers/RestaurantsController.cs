using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class RestaurantsController(IRestaurantRepository restaurantRepository, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants()
    {
        var restaurants = await restaurantRepository.GetRestaurantsAsync();

        var restaurantsToReturn = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        return Ok(restaurantsToReturn);
    }

}

