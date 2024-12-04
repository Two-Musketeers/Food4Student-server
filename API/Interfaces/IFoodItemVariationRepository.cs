using API.Entities;

namespace API.Interfaces;

public interface IFoodItemVariationRepository
{
    Task<IEnumerable<FoodItemVariation>> GetFoodItemVariationsAsync();
    Task<IEnumerable<FoodItemVariation>> GetVariationsByFoodItemIdAsync(string foodItemId);
    Task<FoodItemVariation?> GetFoodItemVariationAsync(string foodItemId, string variationId, string variationOptionId);
    void AddFoodItemVariation(FoodItemVariation foodItemVariation);
    void DeleteFoodItemVariation(FoodItemVariation foodItemVariation);
    Task<bool> SaveAllAsync();
}
