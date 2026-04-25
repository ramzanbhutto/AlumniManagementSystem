using AlumniManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;

namespace AlumniManagementSystem.Domain;

public class User
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Alumni;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }

    // Navigation
    public Alumni? AlumniProfile { get; set; }
    public ICollection<Event> OrganizedEvents { get; set; } = [];
    public ICollection<Newsletter> Newsletters { get; set; } = [];
    public ICollection<Message> SentMessages { get; set; } = [];
    public ICollection<Message> ReceivedMessages { get; set; } = [];
    public ICollection<Survey> Surveys { get; set; } = [];
    public ICollection<NewsletterRecipient> NewsletterRecipients { get; set; } = [];
}
