using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

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
            .Include(r => r.Logo)
            .Include(r => r.Banner)
            .Include(r => r.FoodCategories)
                .ThenInclude(fc => fc.FoodItems)
                    .ThenInclude(fi => fi.Variations)
                        .ThenInclude(v => v.VariationOptions)
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
            .Include(r => r.Logo)
            .Include(r => r.Banner)
            .Include(r => r.Ratings)
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

    public async Task<Restaurant?> GetRestaurantWithDetailsAsync(string id)
    {
        return await context.Restaurants
            .Include(r => r.FoodCategories)
                .ThenInclude(fc => fc.FoodItems)
                    .ThenInclude(fi => fi.Variations)
                        .ThenInclude(fiv => fiv.VariationOptions)
            .Include(r => r.FoodCategories)
                .ThenInclude(fc => fc.FoodItems)
                    .ThenInclude(fi => fi.Variations)
                        .ThenInclude(fiv => fiv.VariationOptions)
            .Include(r => r.Ratings)
            // Include Logo and Banner photos
            .Include(r => r.Logo)
            .Include(r => r.Banner)
            // Include FoodItemPhotos
            .Include(r => r.FoodCategories)
                .ThenInclude(fc => fc.FoodItems)
                    .ThenInclude(fi => fi.FoodItemPhoto)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<PagedList<Restaurant>> GetNearbyRestaurantsAsync(double latitude, double longitude, int pageSize)
    {
        var userLocation = new Point(longitude, latitude) { SRID = 4326 };
        double radiusKm = 3.0;
        List<Restaurant> nearbyRestaurants = [];

        while (true)
        {
            var query = context.Restaurants
                .Where(r => r.IsApproved && r.Location != null)
                .Where(r => r.Location.Distance(userLocation) <= radiusKm * 1000) // Distance in meters
                .OrderBy(r => r.Location.Distance(userLocation))
                .AsQueryable();

            nearbyRestaurants = await query.ToListAsync();

            if (nearbyRestaurants.Count >= pageSize || radiusKm > 50)
            {
                break;
            }

            radiusKm += 2;
        }

        var pagedRestaurants = nearbyRestaurants.Take(pageSize).ToList();

        return await PagedList<Restaurant>.CreateAsync(pagedRestaurants.AsQueryable(), 1, pageSize);
    }
}
