using API.Entities;

namespace API.Interfaces;

public interface IVariationOptionRepository
{
    Task<IEnumerable<VariationOption>> GetVariationOptionsByVariationIdAsync(string variationId);
    Task<VariationOption?> GetVariationOptionByIdAsync(string variationOptionId);
    void AddVariationOption(VariationOption variationOption);
    void DeleteVariationOption(VariationOption variationOption);
    Task<bool> SaveAllAsync();
}
