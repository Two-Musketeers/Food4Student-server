using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class RoleService(DataContext context)
{

    public async Task<List<AppRole>> GetRolesAsync()
    {
        return await context.Roles.ToListAsync();
    }

    public async Task<AppRole> GetRoleByIdAsync(string roleId)
    {
        return await context.Roles.FindAsync(roleId);
    }

    public async Task<AppRole> CreateRoleAsync(string roleName)
    {
        var role = new AppRole { Name = roleName };
        context.Roles.Add(role);
        await context.SaveChangesAsync();
        return role;
    }

    public async Task<AppUser> AssignRoleToUserAsync(string userId, string roleId)
    {
        var user = await context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == userId);
        var role = await context.Roles.FindAsync(roleId);

        if (user == null || role == null)
        {
            throw new Exception("User or Role not found");
        }

        user.UserRoles.Add(new AppUserRole { UserId = userId, RoleId = roleId });
        await context.SaveChangesAsync();
        return user;
    }
}
