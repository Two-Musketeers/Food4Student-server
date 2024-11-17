using System;
using API.Entities;

namespace API.Interfaces;

public interface IFoodItemRepository
{
    void AddFoodItem(FoodItem foodItem);
    void UpdateFoodItem(FoodItem foodItem);
    void DeleteFoodItem(FoodItem foodItem);
    Task<FoodItem?> GetFoodItemByIdAsync(string id);
    Task<IEnumerable<FoodItem>> GetFoodItemsByRestaurantIdAsync(string restaurantId);
    Task<bool> SaveAllAsync();
}
