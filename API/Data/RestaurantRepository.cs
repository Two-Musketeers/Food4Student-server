using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class RestaurantRepository(DataContext context) : IRestaurantRepository
{
    public async Task<Restaurant?> GetRestaurantByIdAsync(int id)
    {
        return await context.Restaurants
            .Include(r => r.Menu)
            .SingleOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Restaurant?> GetRestaurantByNameAsync(string name)
    {
        return await context.Restaurants
            .Include(r => r.Menu)
            .SingleOrDefaultAsync(r => r.Name == name);
    }

    public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync()
    {
        return await context.Restaurants.ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Update(Restaurant restaurant)
    {
        context.Entry(restaurant).State = EntityState.Modified;
    }
}
