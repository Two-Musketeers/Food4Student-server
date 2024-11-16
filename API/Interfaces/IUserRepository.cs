using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<UserDto>> GetMembersAsync(PaginationParams paginationParams);
    Task<AppUser> GetUserByIdAsync(string id);
    Task<AppUser?> GetUserByUsernameAsync(string username);
    Task<AppUser?> GetMemberAsync(string userid);
    void AddUser(AppUser user);
    Task<bool> SaveAllAsync();
    Task<bool> UserExists(string userId);
    Task AddRoleToUserAsync(string userId, string roleName);
}