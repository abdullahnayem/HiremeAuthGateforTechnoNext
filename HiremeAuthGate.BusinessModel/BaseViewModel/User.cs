using System.ComponentModel.DataAnnotations;

namespace HiremeAuthGate.BusinessModel.BaseViewModel
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLoginAt { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int LoginAttempts { get; set; } = 0;
        
        public DateTime? LockedUntil { get; set; }
        
        // Audit fields
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public string? UpdatedBy { get; set; }
    }
}
