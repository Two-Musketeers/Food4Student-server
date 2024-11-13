using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

// public class RoleService(DataContext context)
// {

//     public async Task<List<AppRole>> GetRolesAsync()
//     {
//         return await context.Roles.ToListAsync();
//     }

//     public async Task<AppRole> GetRoleByIdAsync(string roleId)
//     {
//         return await context.Roles.FindAsync(roleId);
//     }

//     public async Task<AppRole> CreateRoleAsync(string roleName)
//     {
//         var role = new AppRole { Name = roleName };
//         context.Roles.Add(role);
//         await context.SaveChangesAsync();
//         return role;
//     }

//     public async Task<AppRole> UpdateRoleAsync(string roleId, string newRoleName)
//     {
//         var role = await context.Roles.FindAsync(roleId) ?? throw new Exception("Role not found");
//         role.Name = newRoleName;
//         await context.SaveChangesAsync();
//         return role;
//     }

//     public async Task<bool> DeleteRoleAsync(string roleId)
//     {
//         var role = await context.Roles.FindAsync(roleId) ?? throw new Exception("Role not found");
//         context.Roles.Remove(role);
//         await context.SaveChangesAsync();
//         return true;
//     }

//     public async Task<AppUser> AssignRoleToUserAsync(string userId, string roleId)
//     {
//         var user = await context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == userId);
//         var role = await context.Roles.FindAsync(roleId);

//         if (user == null || role == null)
//         {
//             throw new Exception("User or Role not found");
//         }

//         user.UserRoles.Add(new AppUserRole { UserId = userId, RoleId = roleId });
//         await context.SaveChangesAsync();
//         return user;
//     }

//     public async Task<AppUser> RemoveRoleFromUserAsync(string userId, string roleId)
//     {
//         var user = await context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == userId) ?? throw new Exception("User not found");
//         var userRole = await context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
//         if (userRole == null)
//         {
//             throw new Exception("Role not assigned to user");
//         }

//         user.UserRoles.Remove(userRole);
//         await context.SaveChangesAsync();
//         return user;
//     }
// }
