using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class RestaurantRepository(DataContext context, IMapper mapper) : IRestaurantRepository
{
    public async Task<RestaurantDto?> GetRestaurantByIdAsync(string id)
    {
        return await context.Restaurants
            .Where(r => r.Id == id)
            .ProjectTo<RestaurantDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<Restaurant?> GetRestaurantByNameAsync(string name)
    {
        return await context.Restaurants
            .Include(r => r.Menu)
            .Include(r => r.Logo)
            .Include(r => r.Banner)
            .SingleOrDefaultAsync(r => r.Name == name);
    }
    public async Task<PagedList<RestaurantDto>> GetRestaurantsAsync(PaginationParams paginationParams)
    {
        var query = context.Restaurants
            .Include(r => r.Menu)
                .ThenInclude(m => m.FoodItemPhoto)
            .Include(r => r.Logo)
            .Include(r => r.Banner)
            .ProjectTo<RestaurantDto>(mapper.ConfigurationProvider)
            .AsQueryable();

        return await PagedList<RestaurantDto>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public Task<IEnumerable<Restaurant>> GetRestaurantsAsync()
    {
        throw new NotImplementedException();
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
