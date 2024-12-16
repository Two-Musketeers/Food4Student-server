using API.Entities;

namespace API.Interfaces;

public interface IVariationOptionRepository
{
    Task<VariationOption?> GetVariationOptionByIdAsync(string id);
    void AddVariationOption(VariationOption variationOption);
    void RemoveVariationOption(VariationOption variationOption);
    Task<bool> SaveAllAsync();
}
