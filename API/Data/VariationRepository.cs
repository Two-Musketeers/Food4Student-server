using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class VariationRepository(DataContext context) : IVariationRepository
{
    public void AddVariation(Variation variation)
    {
        context.Variations.Add(variation);
    }

    public async Task<Variation?> GetVariationByIdAsync(string id)
    {
        return await context.Variations
            .Include(v => v.FoodItem)
                .ThenInclude(f => f.FoodCategory)
            .Include(v => v.VariationOptions)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Variation>> GetVariationsByIdsAsync(List<string> ids)
    {
        return await context.Variations
                .Include(v => v.VariationOptions)
                .Where(v => ids.Contains(v.Id))
                .ToListAsync();
    }

    public void RemoveVariation(Variation variation)
    {
        context.Variations.Remove(variation);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
