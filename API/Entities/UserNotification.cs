using System;

namespace API.Entities;

public class UserNotification
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Image { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public bool IsUnread { get; set; } = true;

    public string? AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
}
