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
        return await context.Variations.FirstOrDefaultAsync(x => x.Id == id);
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
