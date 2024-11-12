using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<UserDto>> GetMembersAsync(PaginationParams paginationParams);
    Task<AppUser> GetUserByUsernameAsync(string username);
    Task<bool> UserRoleExistsAsync(string userId, string roleId);
    Task AddUserAsync(AppUser user);
    Task AddRoleToUserAsync(string userId, string roleId);
}