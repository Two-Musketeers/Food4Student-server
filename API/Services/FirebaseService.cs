using System;
using API.Interfaces;

namespace API.Services;

public class FirebaseService : IFirebaseService
{
    public async Task AssignRoleAsync(string uid, string role)
    {
        var claims = new Dictionary<string, object> { { "role", role } };
        await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(uid, claims);
    }
}
