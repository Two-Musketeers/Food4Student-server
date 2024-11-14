using System;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class RoleRepository(DataContext context) : IRoleRepository
{
    public async Task<AppRole> GetRoleByNameAsync(string roleName)
    {
        return await context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
    }
}
