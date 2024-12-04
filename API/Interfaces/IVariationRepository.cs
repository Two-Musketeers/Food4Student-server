using API.Entities;

namespace API.Interfaces;

public interface IVariationRepository
{
    Task<IEnumerable<Variation>> GetVariationsByRestaurantIdAsync(string restaurantId);
    Task<Variation?> GetVariationByIdAsync(string variationId);
    void AddVariation(Variation variation);
    void DeleteVariation(Variation variation);
    Task<bool> SaveAllAsync();
}
