using API.Entities;

namespace API.Interfaces;

public interface IFoodItemRepository
{
    void AddFoodItem(FoodItem foodItem);
    void UpdateFoodItem(FoodItem foodItem);
    void DeleteFoodItem(FoodItem foodItem);
    Task<IEnumerable<FoodItem>> GetFoodItemsByCategoryIdAsync(string foodCategoryId);
    Task<FoodItem?> GetFoodItemByIdAsync(string id);
    Task<IEnumerable<FoodItem>> GetFoodItemsByRestaurantIdAsync(string restaurantId);
    Task<FoodItem?> GetFoodItemByIdDirectlyAsync(string id);
    Task<IEnumerable<FoodItem>> GetAllFoodItemsByRestaurantAsync(string restaurantId);
    Task<FoodItem?> GetFoodItemWithCategoryAsync(string id);
    Task<IEnumerable<FoodItem>> GetFoodItemsWithCategoryAsync(IEnumerable<string> foodItemIds);
    Task<bool> SaveAllAsync();
}
