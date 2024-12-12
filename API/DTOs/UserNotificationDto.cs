using System;

namespace API.DTOs;

public class UserNotificationDto
{
    public string? Id { get; set; }
    public string? Image { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsUnread { get; set; }
}
