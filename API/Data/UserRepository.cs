using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class UserRepository(DataContext context) : IUserRepository
{
    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.DisplayName == username);
    }

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

    Task<IEnumerable<UserDto>> IUserRepository.GetMembersAsync(PaginationParams paginationParams)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UserExists(string userId)
    {
        return await context.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task<AppUser> GetUserByIdAsync(string id)
    {
        var user = await context.Users
            .Include(u => u.ShippingAddresses)
            .Include(u => u.OwnedRestaurant)
                .ThenInclude(r => r.Menu)
                    .ThenInclude(m => m.FoodItemPhoto)
            .SingleOrDefaultAsync(u => u.Id == id)
                ?? throw new Exception("User not found");
        return user;
    }
}