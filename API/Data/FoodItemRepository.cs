using System;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class FoodItemRepository(DataContext context) : IFoodItemRepository
{
    public void AddFoodItem(FoodItem foodItem)
    {
        context.FoodItems.Add(foodItem);
    }

    public void DeleteFoodItem(FoodItem foodItem)
    {
        context.FoodItems.Remove(foodItem);
    }

    public async Task<FoodItem?> GetFoodItemByIdAsync(string id)
    {
        return await context.FoodItems
            .Include(f => f.FoodItemPhoto)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<FoodItem>> GetFoodItemsByCategoryIdAsync(string foodCategoryId)
    {
        return await context.FoodItems
            .Where(fi => fi.FoodCategoryId == foodCategoryId)
            .Include(fi => fi.FoodItemPhoto)
            .ToListAsync();
    }

    public async Task<IEnumerable<FoodItem>> GetFoodItemsByRestaurantIdAsync(string restaurantId)
    {
        return await context.FoodItems
            .Where(f => f.RestaurantId == restaurantId)
            .Include(f => f.FoodItemPhoto)
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void UpdateFoodItem(FoodItem foodItem)
    {
        context.FoodItems.Update(foodItem);
    }
}
