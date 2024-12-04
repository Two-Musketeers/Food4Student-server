using System;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class FoodCategoryRepository(DataContext context) : IFoodCategoryRepository
{
    public void AddFoodCategory(FoodCategory category)
    {
        context.FoodCategories.Add(category);
    }

    public async Task<IEnumerable<FoodCategory?>> GetFoodCategoriesAsync(string restaurantId)
    {
        return await context.FoodCategories
            .Where(r => r.RestaurantId == restaurantId).ToListAsync();
    }

    public async Task<FoodCategory?> GetFoodCategoryAsync(string FoodCategoryId)
    {
        return await context.FoodCategories
            .Include(fc => fc.FoodItems)
                .ThenInclude(f => f.FoodItemPhoto)
            .FirstOrDefaultAsync(fc => fc.Id == FoodCategoryId);
    }

    public void RemoveFoodCategory(FoodCategory foodCategory)
    {
        context.FoodCategories.Remove(foodCategory);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void UpdateFoodCategory(FoodCategory foodCategory)
    {
        context.FoodCategories.Update(foodCategory);
    }
    
    public void RemoveFoodItemFromCategory(FoodCategory category, FoodItem foodItem)
    {
        category.FoodItems.Remove(foodItem);
    }
}
