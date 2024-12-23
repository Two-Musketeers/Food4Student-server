using API.Entities;

namespace API.Interfaces;

public interface IVariationOptionRepository
{
    Task<VariationOption?> GetVariationOptionByIdAsync(string id);
    Task<List<VariationOption>> GetVariationOptionsByIdsAsync(List<string> ids);
    void AddVariationOption(VariationOption variationOption);
    void RemoveVariationOption(VariationOption variationOption);
    Task<bool> SaveAllAsync();
}
