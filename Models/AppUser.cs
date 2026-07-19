using System.ComponentModel.DataAnnotations;

namespace ScamShieldAI.Models
{
    // جدول Users (تبسيط بدون ASP.NET Identity الكامل، قابل للترقية لاحقًا)
    public class AppUser
    {
        [Key] // <-- أضيفي هذا السطر هنا لتحديد المفتاح الأساسي
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}