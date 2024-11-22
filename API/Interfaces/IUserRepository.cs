using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<UserDto>> GetMembersAsync(PaginationParams paginationParams);
    Task<AppUser> GetUserByIdAsync(string id);
    Task<AppUser?> GetMemberAsync(string userid);
    void AddUser(AppUser user);
    void RemoveUser(AppUser user);
    Task<bool> SaveAllAsync();
    Task<bool> UserExists(string userId);
}