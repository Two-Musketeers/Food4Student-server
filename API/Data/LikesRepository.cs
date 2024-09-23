using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LikesRepository(DataContext context, IMapper mapper) : ILikeRepository
{
    public void AddRestaurantLike(RestaurantLike restaurantLike)
    {
        context.RestaurantLikes.Add(restaurantLike);
    }

    public void RemoveRestaurantLike(RestaurantLike restaurantLike)
    {
        context.RestaurantLikes.Remove(restaurantLike);
    }

    public async Task<PagedList<LikeRestaurantDto>> GetRestaurantLikesAsync(LikesParams likesParams)
    {
        var query = context.RestaurantLikes
            .Where(like => like.SourceUserId == likesParams.UserId)
            .Select(like => like.LikedRestaurant)
            .ProjectTo<LikeRestaurantDto>(mapper.ConfigurationProvider)
            .AsQueryable();

        return await PagedList<LikeRestaurantDto>.CreateAsync(query, likesParams.PageNumber, likesParams.PageSize);
    }

    public async Task<bool> ToggleRestaurantLikeAsync(string userId, string restaurantId)
    {
        var like = await context.RestaurantLikes
            .FirstOrDefaultAsync(l => l.SourceUserId == userId && l.LikedRestaurantId == restaurantId);

        if (like == null)
        {
            var restaurantLike = new RestaurantLike
            {
                SourceUserId = userId,
                LikedRestaurantId = restaurantId
            };
            context.RestaurantLikes.Add(restaurantLike);
        }
        else
        {
            context.RestaurantLikes.Remove(like);
        }

        return await context.SaveChangesAsync() > 0;
    }
}
