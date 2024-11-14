using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<UserDto>> GetMembersAsync(PaginationParams paginationParams);
    Task<AppUser?> GetUserByIdAsync(string id);
    Task<AppUser?> GetUserByUsernameAsync(string username);
    Task<AppUser?> GetMemberAsync(string userid);
    Task<bool> AddUserAsync(AppUser user);
    Task AddRoleToUserAsync(string userId, string roleName);
}