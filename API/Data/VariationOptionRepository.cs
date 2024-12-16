using API.Entities;
using API.Interfaces;

namespace API.Data;

public class VariationOptionRepository(DataContext context) : IVariationOptionRepository
{
    public void AddVariationOption(VariationOption variationOption)
    {
        context.VariationOptions.Add(variationOption);
    }

    public async Task<VariationOption?> GetVariationOptionByIdAsync(string id)
    {
        return await context.VariationOptions.FindAsync(id);
    }

    public void RemoveVariationOption(VariationOption variationOption)
    {
        context.VariationOptions.Remove(variationOption);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
