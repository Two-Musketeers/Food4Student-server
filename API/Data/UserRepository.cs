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
        return await context.Users.SingleOrDefaultAsync(u => u.UserName == username);
    }

    public Task AddUserAsync(AppUser user)
    {
        context.Users.Add(user);
        return Task.CompletedTask;
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public async Task AddRoleToUserAsync(string userId, string roleName)
    {
        var user = await context.Users.Include(u => u.AppRole).SingleOrDefaultAsync(u => u.Id == userId);
        var role = await context.Roles.SingleOrDefaultAsync(r => r.Name == roleName);

        if (user == null || role == null)
        {
            throw new Exception("User or Role not found");
        }

        user.AppRole = role;
        await context.SaveChangesAsync();
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

    public async Task<AppUser?> GetUserByIdAsync(string id)
    {
        var user = await context.Users.FindAsync(id);
        return user;
    }
}