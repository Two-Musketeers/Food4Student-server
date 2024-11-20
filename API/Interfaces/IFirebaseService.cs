namespace API.Interfaces;

public interface IFirebaseService
{
    Task AssignRoleAsync(string uid, string role);
    Task<string> GetUserRoleAsync(string uid);
}
