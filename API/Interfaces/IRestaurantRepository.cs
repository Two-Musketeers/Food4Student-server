using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IRestaurantRepository
{
    void Update(Restaurant restaurant);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<Restaurant>> GetRestaurantsAsync();
    Task<RestaurantDto?> GetRestaurantByIdAsync(string id);
    Task<Restaurant?> GetRestaurantByNameAsync(string name);
    Task<PagedList<RestaurantDto>> GetRestaurantsAsync(PaginationParams paginationParams);
    
}
