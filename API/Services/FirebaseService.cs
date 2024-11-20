using System;
using API.Interfaces;
using FirebaseAdmin.Auth;

namespace API.Services;

public class FirebaseService : IFirebaseService
{
    public async Task AssignRoleAsync(string uid, string role)
    {
        var claims = new Dictionary<string, object> { { "role", role } };
        await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(uid, claims);
    }
    public async Task<string> GetUserRoleAsync(string uid)
    {
        var user = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);

        if (user.CustomClaims != null && user.CustomClaims.TryGetValue("role", out var roleObj))
        {
            return roleObj.ToString() ?? throw new Exception("Role is null");
        }

        return "NotRegistered";
    }
}
