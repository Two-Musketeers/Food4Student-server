using System;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class FoodItemVariationRepository(DataContext context) : IFoodItemVariationRepository
{
    public async Task<IEnumerable<FoodItemVariation>> GetFoodItemVariationsAsync()
    {
        return await context.FoodItemVariations
            .Include(fiv => fiv.FoodItem)
            .Include(fiv => fiv.Variation)
            .Include(fiv => fiv.VariationOption)
            .ToListAsync();
    }

    public void AddFoodItemVariation(FoodItemVariation foodItemVariation)
    {
        context.FoodItemVariations.Add(foodItemVariation);
    }

    public void DeleteFoodItemVariation(FoodItemVariation foodItemVariation)
    {
        context.FoodItemVariations.Remove(foodItemVariation);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<FoodItemVariation?> GetFoodItemVariationAsync(string foodItemId, string variationId, string variationOptionId)
    {
        return await context.FoodItemVariations
            .Include(fiv => fiv.FoodItem)
            .Include(fiv => fiv.Variation)
            .FirstOrDefaultAsync(fiv => fiv.FoodItemId == foodItemId
                && fiv.VariationId == variationId
                && fiv.VariationOptionId == variationOptionId);
    }

    public async Task<IEnumerable<FoodItemVariation>> GetVariationsByFoodItemIdAsync(string foodItemId)
    {
        return await context.FoodItemVariations
            .Where(fiv => fiv.FoodItemId == foodItemId)
            .Include(fiv => fiv.Variation)
            .ThenInclude(v => v.VariationOptions)
            .Include(fiv => fiv.VariationOption)
            .ToListAsync();
    }
}
