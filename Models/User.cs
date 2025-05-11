using Microsoft.AspNetCore.Identity;
using System;

namespace BirSiberDanismanlik.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Role { get; set; }
        public string? FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
} 