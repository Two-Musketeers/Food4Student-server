using API.Entities;

namespace API.Interfaces;

public interface IRatingRepository
{
    Task<double> GetAverageRatingAsync(string restaurantId);
    Task<bool> SaveAllAsync();
    Task<List<Rating>> GetUserRatingsAsync(string userId);
    Task<List<Rating>> GetRestaurantRatingsAsync(string restaurantId);
    Task<Rating?> GetOrderRatingById(string id);
    void AddRating(Rating rating);
    void UpdateRating(Rating rating);
    void DeleteRating(Rating rating);
}
