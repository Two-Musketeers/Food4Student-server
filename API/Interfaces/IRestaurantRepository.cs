using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IRestaurantRepository
{
    void Update(Restaurant restaurant);
    void AddRestaurant(Restaurant restaurant);
    void DeleteRestaurant(Restaurant restaurant);
    Task<bool> SaveAllAsync();
    Task<Restaurant?> GetRestaurantByIdAsync(string id);
    Task<Restaurant?> GetRestaurantByNameAsync(string name);
    Task<PagedList<Restaurant>> GetRestaurantsAsync(PaginationParams paginationParams);
    Task<PagedList<Restaurant>> GetApprovedRestaurantsAsync(PaginationParams paginationParams);
}
