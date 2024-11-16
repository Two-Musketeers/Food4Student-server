using System;

namespace API.Interfaces;

public interface IFirebaseService
{
    Task AssignRoleAsync(string uid, string role);
}
