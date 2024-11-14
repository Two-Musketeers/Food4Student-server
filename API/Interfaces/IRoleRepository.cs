using System;
using API.Entities;

namespace API.Interfaces;

public interface IRoleRepository
{
    Task<AppRole> GetRoleByNameAsync(string roleName);
}
