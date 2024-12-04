using API.Entities;

namespace API.Interfaces;

public interface IFoodCategoryRepository
{
    Task<IEnumerable<FoodCategory?>> GetFoodCategoriesAsync(string restaurantId);
    Task<FoodCategory?> GetFoodCategoryAsync(string FoodCategoryId);
    void RemoveFoodItemFromCategory(FoodCategory category, FoodItem foodItem);
    void UpdateFoodCategory(FoodCategory foodCategory);
    void AddFoodCategory(FoodCategory category);
    void RemoveFoodCategory(FoodCategory foodCategory);
    Task<bool> SaveAllAsync();
}
