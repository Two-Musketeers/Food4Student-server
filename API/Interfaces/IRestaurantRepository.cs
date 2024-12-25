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
    Task<Restaurant?> GetRestaurantWithoutAnyInfoByIdAsync(string id);
    Task<Restaurant?> GetRestaurantByNameAsync(string name); //Will use to query restaurant in the future
    Task<PagedList<Restaurant>> GetRestaurantsAsync(PaginationParams paginationParams);
    Task<PagedList<Restaurant>> GetApprovedRestaurantsAsync(PaginationParams paginationParams);
    Task<Restaurant?> GetRestaurantWithCategoriesAsync(string restaurantId);
    Task<Restaurant?> GetRestaurantWithDetailsAsync(string id);
    Task<PagedList<Restaurant>> GetNearbyRestaurantsAsync(double latitude, double longitude, int pageSize, int pageNumber);
    Task<PagedList<Restaurant>> SearchRestaurantsByNameAsync(string query, int pageNumber, int pageSize);
}
