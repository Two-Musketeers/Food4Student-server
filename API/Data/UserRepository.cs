using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<bool> UserRoleExistsAsync(string userId, string roleId)
    {
        return await context.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }

    public async Task AddUserAsync(AppUser user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task AddRoleToUserAsync(string userId, string roleId)
    {
        var userRole = new AppUserRole { UserId = userId, RoleId = roleId };
        context.UserRoles.Add(userRole);
        await context.SaveChangesAsync();
    }

    public async Task<AppUser?> GetMemberAsync(string username)
    {
        return await context.Users
            .Where(x => x.UserName == username)
            .SingleOrDefaultAsync();
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await context.Users
            .Include(x => x.Avatar)
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }

    Task<IEnumerable<UserDto>> IUserRepository.GetMembersAsync(PaginationParams paginationParams)
    {
        throw new NotImplementedException();
    }
}