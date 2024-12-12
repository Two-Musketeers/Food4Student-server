using System;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserNotificationRepository(DataContext context) : IUserNotificationRepository
{
    public void AddUserNotification(UserNotification userNotification)
    {
        context.UserNotifications.Add(userNotification);
    }

    public async Task<UserNotification?> GetUserNotificationByIdAsync(string id)
    {
        return await context.UserNotifications.FindAsync(id);
    }

    public async Task<List<UserNotification>> GetUserNotificationsAsync(string userId)
    {
        return await context.UserNotifications
            .Where(un => un.AppUserId == userId)
            .OrderByDescending(un => un.Timestamp)
            .ToListAsync();
    }

    public void RemoveUserNotification(UserNotification userNotification)
    {
        context.UserNotifications.Remove(userNotification);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
