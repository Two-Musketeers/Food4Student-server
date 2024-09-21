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
            .Include(r => r.Logo)
            .Include(r => r.Banner)
            .SingleOrDefaultAsync(r => r.Name == name);
    }

    public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync()
    {
        return await context.Restaurants
            .Include(r => r.Menu)
                .ThenInclude(m => m.FoodItemPhoto)
            .Include(r => r.Logo)
            .Include(r => r.Banner)
            .ToListAsync();
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
