using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class UserRepository(DataContext context) : IUserRepository
{
    public void AddUser(AppUser user)
    {
        context.Users.Add(user);
    }

    public void RemoveUser(AppUser user)
    {
        context.Users.Remove(user);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<AppUser?> GetMemberAsync(string userid)
    {
        return await context.Users
            .Where(x => x.Id == userid)
            .SingleOrDefaultAsync();
    }

    public async Task<PagedList<AppUser>> GetMembersAsync(PaginationParams paginationParams)
    {
        var query = context.Users
            .Include(u => u.OwnedRestaurant)
            .AsQueryable();
        return await PagedList<AppUser>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<bool> UserExists(string userId)
    {
        return await context.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task<AppUser> GetUserByIdAsync(string id)
    {
        var user = await context.Users
            .Include(u => u.FavoriteRestaurants)
            .Include(u => u.UserNotifications)
            .Include(u => u.ShippingAddresses)
            .Include(u => u.DeviceTokens)
            .Include(u => u.OwnedRestaurant)
            .SingleOrDefaultAsync(u => u.Id == id)
                ?? throw new Exception("User not found");

        if (user.OwnedRestaurant != null)
        {
            await context.Entry(user.OwnedRestaurant)
                .Collection(r => r.FoodCategories)
                .Query()
                .Include(m => m.FoodItems)
                    .ThenInclude(f => f.FoodItemPhoto)
                .LoadAsync();
        }

        return user;
    }
    
    public async Task<bool> TokenExists(string token, string userId)
    {
        var deviceToken = await context.DeviceTokens.AnyAsync(t => t.Token == token && t.AppUserId == userId);
        if (deviceToken) return true;
        return false;
    }

    public async Task<List<string>> GetDeviceTokens(string userId)
    {
        var tokenList = await context.DeviceTokens.Where(t => t.AppUserId == userId).ToListAsync();
        return tokenList.Select(t => t.Token).ToList();
    }

    public async Task<AppUser> GetUserWithRestaurantByIdAsync(string id)
    {
        return await context.Users
            .Include(u => u.OwnedRestaurant)
            .SingleOrDefaultAsync(u => u.Id == id)
                ?? throw new Exception("User not found");
    }
}