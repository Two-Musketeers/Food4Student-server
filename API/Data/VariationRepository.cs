using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class VariationRepository(DataContext context) : IVariationRepository
{
    public async Task<IEnumerable<Variation>> GetVariationsByRestaurantIdAsync(string restaurantId)
    {
        return await context.Variations
            .Include(v => v.VariationOptions)
            .Where(v => v.RestaurantId == restaurantId)
            .ToListAsync();
    }

    public async Task<Variation?> GetVariationByIdAsync(string variationId)
    {
        return await context.Variations
            .Include(v => v.VariationOptions)
            .FirstOrDefaultAsync(v => v.Id == variationId);
    }

    public void AddVariation(Variation variation)
    {
        context.Variations.Add(variation);
    }

    public void DeleteVariation(Variation variation)
    {
        context.Variations.Remove(variation);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
