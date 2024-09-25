using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<PagedList<UserDto>> GetMembersAsync(PaginationParams paginationParams);
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?> GetUserByUsernameAsync(string username);
}