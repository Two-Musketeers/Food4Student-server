using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class VariationOptionRepository(DataContext context) : IVariationOptionRepository
{
    public async Task<IEnumerable<VariationOption>> GetVariationOptionsByVariationIdAsync(string variationId)
    {
        return await context.VariationOptions
            .Where(vo => vo.VariationId == variationId)
            .ToListAsync();
    }

    public async Task<VariationOption?> GetVariationOptionByIdAsync(string variationOptionId)
    {
        return await context.VariationOptions
            .FirstOrDefaultAsync(vo => vo.Id == variationOptionId);
    }

    public void AddVariationOption(VariationOption variationOption)
    {
        context.VariationOptions.Add(variationOption);
    }

    public void DeleteVariationOption(VariationOption variationOption)
    {
        context.VariationOptions.Remove(variationOption);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
