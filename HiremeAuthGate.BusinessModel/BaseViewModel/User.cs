using System.ComponentModel.DataAnnotations;

namespace HiremeAuthGate.BusinessModel.BaseViewModel
{
    /// <summary>
    /// Represents a user entity in the authentication system.
    /// Contains user information, authentication details, and security tracking.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// Auto-generated using Guid.NewGuid().
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the hashed password for the user.
        /// Required field containing the BCrypt hash of the user's password.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLoginAt { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int LoginAttempts { get; set; } = 0;
        
        public DateTime? LockedUntil { get; set; }
        
       
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Gets or sets the identifier of who last updated the user record.
        /// Nullable field for audit tracking purposes.
        /// </summary>
        public string? UpdatedBy { get; set; }
    }
}
