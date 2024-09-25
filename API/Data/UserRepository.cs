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

    public async Task<AppUser?> GetMemberAsync(string username)
    {
        return await context.Users
            .Where(x => x.UserName == username)
            .SingleOrDefaultAsync();
    }

    public async Task<PagedList<UserDto>> GetMembersAsync(PaginationParams paginationParams)
    {
        var query = context.Users
            .Include(x => x.OwnedRestaurants)
            .Include(x => x.FavoriteRestaurants)
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .AsQueryable();

        return await PagedList<UserDto>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await context.Users
            .Include(x => x.Avatar)
            .SingleOrDefaultAsync(x => x.UserName == username);
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
}