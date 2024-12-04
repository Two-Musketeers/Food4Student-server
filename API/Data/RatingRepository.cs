using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class RatingRepository(DataContext context) : IRatingRepository
{
    public async Task<double> GetAverageRatingAsync(string restaurantId)
    {
        var averageRating = await context.Ratings
            .Where(r => r.RestaurantId == restaurantId)
            .AverageAsync(r => r.Stars);

        return averageRating;
    }
    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void AddRating(Rating rating)
    {
        context.Ratings.Add(rating);
    }

    public void UpdateRating(Rating rating)
    {
        context.Ratings.Update(rating);
    }

    public void DeleteRating(Rating rating)
    {
        context.Ratings.Remove(rating);
    }

    public async Task<List<Rating>> GetUserRatingsAsync(string userId)
    {
        var user = await context.Users
            .Include(u => u.Ratings)
            .FirstOrDefaultAsync(u => u.Id == userId)!;
        
        if (user == null) throw new Exception("User not found");

        return user.Ratings;
    }

    public async Task<List<Rating>> GetRestaurantRatingsAsync(string restaurantId)
    {
        var restaurant = await context.Restaurants
            .Include(r => r.Ratings)
            .FirstOrDefaultAsync(r => r.Id == restaurantId)!;
        
        if (restaurant == null) throw new Exception("Restaurant not found");

        return restaurant.Ratings;
    }

    public async Task<Rating?> GetOrderRatingById(string id)
    {
        var order = await context.Orders.FirstOrDefaultAsync(r => r.Id == id);
        if (order == null) throw new Exception("Order not found");
        return order.Rating;
    }
}
