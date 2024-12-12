using API.Entities;

namespace API.Interfaces;

public interface IUserNotificationRepository
{
    void AddUserNotification(UserNotification userNotification);
    void RemoveUserNotification(UserNotification userNotification);
    Task<bool> SaveAllAsync();
    Task<UserNotification?> GetUserNotificationByIdAsync(string id);
    Task<List<UserNotification>> GetUserNotificationsAsync(string userId);
}
