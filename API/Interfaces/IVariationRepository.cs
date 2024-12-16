using API.Entities;

namespace API.Interfaces;

public interface IVariationRepository
{
    void AddVariation(Variation variation);
    void RemoveVariation(Variation variation);
    Task<bool> SaveAllAsync();
    Task<Variation?> GetVariationByIdAsync(string id);
}
