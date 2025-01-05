using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication3.Models;


public class User
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(100)] public string Username { get; private set; }
        [MaxLength(100)] public string Email { get; private set; }

        [MaxLength(100)]
        public string PasswordHash { get; set; } = string.Empty;
        [MaxLength(50)]
        public string? Role { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public ICollection<UserSetting> UserSettings { get; set; } = new List<UserSetting>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        
        public User(string username, string email, string passwordHash)
        {
            Id = Guid.NewGuid(); // Generate a new UUID when a new User object is created
            Username = username; // Set the username
            Email = email; // Set the email (identity) and make it read-only
            PasswordHash = passwordHash; // Set the password hash
            CreateAt = DateTime.UtcNow; // Set CreatedAt to the current time
            UpdateAt = DateTime.UtcNow; 
        }
    }
