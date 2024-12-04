using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class RestaurantRepository(DataContext context) : IRestaurantRepository
{
    public void AddRestaurant(Restaurant restaurant)
    {
        context.Restaurants.Add(restaurant);

        var defaultCategory = new FoodCategory
        {
            Name = "Default",
            RestaurantId = restaurant.Id
        };

        context.FoodCategories.Add(defaultCategory);
    }

    public async Task<Restaurant?> GetRestaurantByIdAsync(string id)
    {
        return await context.Restaurants
            .Include(r => r.FoodCategories)
                .ThenInclude(fc => fc.FoodItems)
                    .ThenInclude(fi => fi.FoodItemPhoto)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<PagedList<Restaurant>> GetRestaurantsAsync(PaginationParams paginationParams)
    {
        var query = context.Restaurants
            .Include(r => r.FoodCategories)
                .ThenInclude(m => m.FoodItems)
                    .ThenInclude(f => f.FoodItemPhoto)
            .Include(r => r.Logo)
            .Include(r => r.Banner)
            .Include(r => r.Ratings)
            .AsQueryable();

        return await PagedList<Restaurant>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
    }
    public async Task<Restaurant?> GetRestaurantByNameAsync(string name)
    {
        return await context.Restaurants
            .Include(r => r.FoodCategories)
                .ThenInclude(m => m.FoodItems)
                    .ThenInclude(f => f.FoodItemPhoto)
            .Include(r => r.Logo)
            .Include(r => r.Banner)
            .SingleOrDefaultAsync(r => r.Name == name);
    }

    public async Task<PagedList<Restaurant>> GetApprovedRestaurantsAsync(PaginationParams paginationParams)
    {
        var query = context.Restaurants
            .Include(r => r.FoodCategories)
                    .ThenInclude(fc => fc.FoodItems)
                        .ThenInclude(fi => fi.FoodItemPhoto)
            .Where(r => r.IsApproved)
            .AsQueryable();

        return await PagedList<Restaurant>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Update(Restaurant restaurant)
    {
        context.Entry(restaurant).State = EntityState.Modified;
    }

    public void DeleteRestaurant(Restaurant restaurant)
    {
        context.Restaurants.Remove(restaurant);
    }

    public async Task<Restaurant?> GetRestaurantWithCategoriesAsync(string restaurantId)
    {
        return await context.Restaurants
        .Include(r => r.FoodCategories)
            .ThenInclude(fc => fc.FoodItems)
                .ThenInclude(fi => fi.FoodItemPhoto)
        .FirstOrDefaultAsync(r => r.Id == restaurantId);
    }
}
