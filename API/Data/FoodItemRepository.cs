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

    public async Task<IEnumerable<FoodItem>> GetAllFoodItemsByRestaurantAsync(string restaurantId)
    {
        return await context.FoodItems
                .Where(fi => fi.FoodCategory.RestaurantId == restaurantId)
                .Include(fi => fi.FoodCategory)
                .Include(fi => fi.Variations)
                    .ThenInclude(v => v.VariationOptions)
                .ToListAsync();
    }

    public async Task<FoodItem?> GetFoodItemByIdAsync(string id)
    {
        return await context.FoodItems
            .Include(f => f.FoodItemPhoto)
            .Include(fi => fi.FoodCategory)
            .Include(fi => fi.Variations)
                .ThenInclude(v => v.VariationOptions)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<FoodItem?> GetFoodItemByIdDirectlyAsync(string id)
    {
        return await context.FoodItems
                .Include(fi => fi.FoodCategory)
                .Include(fi => fi.Variations)
                    .ThenInclude(v => v.VariationOptions)
                .FirstOrDefaultAsync(fi => fi.Id == id);
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
            .Include(fi => fi.FoodCategory)
            .Where(fi => fi.FoodCategory.RestaurantId == restaurantId)
            .ToListAsync();
    }

    public async Task<FoodItem?> GetFoodItemWithCategoryAsync(string id)
    {
        return await context.FoodItems
            .Include(fi => fi.FoodCategory)
            .FirstOrDefaultAsync(fi => fi.Id == id);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void UpdateFoodItem(FoodItem foodItem)
    {
        context.FoodItems.Update(foodItem);
    }

    public async Task<IEnumerable<FoodItem>> GetFoodItemsWithCategoryAsync(List<string> foodItemIds)
    {
        return await context.FoodItems
            .Include(fi => fi.FoodCategory)
            .Include(fi => fi.FoodItemPhoto)
            .Where(fi => foodItemIds.Contains(fi.Id))
            .ToListAsync();
    }

    public async Task<FoodItem?> GetFoodItemWithVariationsAsync(string foodItemId)
    {
        return await context.FoodItems
            .Include(fi => fi.Variations)
                .ThenInclude(v => v.VariationOptions)
            .FirstOrDefaultAsync(fi => fi.Id == foodItemId);
    }
}
