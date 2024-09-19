using API.Entities;

namespace API.Interfaces;

public interface IRestaurantRepository
{
    void Update(Restaurant restaurant);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<Restaurant>> GetRestaurantsAsync();
    Task<Restaurant?> GetRestaurantByIdAsync(int id);
    Task<Restaurant?> GetRestaurantByNameAsync(string name);
    
}
