using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface ILikeRepository
{
    Task<PagedList<LikeRestaurantDto>> GetRestaurantLikesAsync(LikesParams likesParams);
    void AddRestaurantLike(RestaurantLike restaurantLike);
    void RemoveRestaurantLike(RestaurantLike restaurantLike);
    Task<bool> ToggleRestaurantLikeAsync(string userId, string restaurantId);
}
