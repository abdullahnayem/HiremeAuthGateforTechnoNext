using HiremeAuthGate.BusinessModel.BaseViewModel;
using HiremeAuthGate.BusinessModel.Results;
using HiremeAuthGate.Services.Interfaces;

namespace HiremeAuthGate.Services.Services
{
    /// <summary>
    /// Service implementation for authentication operations.
    /// Handles user authentication, registration, and account security features.
    /// </summary>
    public class AuthService(IUserRepository users) : IAuthService
    {
        /// <summary>
        /// Maximum number of failed login attempts before account lockout.
        /// </summary>
        private const int MaxLoginAttempts = 5;
        
        /// <summary>
        /// Duration in minutes for account lockout after exceeding max login attempts.
        /// </summary>
        private const int LockoutDurationMinutes = 15;

        /// <summary>
        /// Authenticates a user with the provided email and password.
        /// Implements account locking mechanism and security features.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The password to authenticate.</param>
        /// <param name="ct">Cancellation token for async operation.</param>
        /// <returns>A result containing the authenticated user or error information.</returns>
        public async Task<Result<User>> AuthenticateAsync(string email, string password, CancellationToken ct = default)
        {
            try
            {
                var existing = await users.GetByEmailAsync(email, ct);
                if (existing is null)
                {
                    return Result<User>.Failure("Invalid email or password.");
                }

                // Check if account is locked
                if (existing.LockedUntil.HasValue && existing.LockedUntil.Value > DateTime.UtcNow)
                {
                    var remainingTime = existing.LockedUntil.Value - DateTime.UtcNow;
                    return Result<User>.Failure($"Account is locked. Please try again in {remainingTime.Minutes} minutes.");
                }

                // Check if account is active
                if (!existing.IsActive)
                {
                    return Result<User>.Failure("Account is deactivated. Please contact support.");
                }

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(password, existing.PasswordHash))
                {
                    // Increment login attempts
                    existing.LoginAttempts++;
                    
                    // Lock account if max attempts reached
                    if (existing.LoginAttempts >= MaxLoginAttempts)
                    {
                        existing.LockedUntil = DateTime.UtcNow.AddMinutes(LockoutDurationMinutes);
                        existing.LoginAttempts = 0;
                    }
                    
                    existing.UpdatedAt = DateTime.UtcNow;
                    await users.UpdateAsync(existing, ct);
                    await users.SaveChangesAsync(ct);
                    
                    return Result<User>.Failure("Invalid email or password.");
                }

                // Reset login attempts on successful login
                existing.LoginAttempts = 0;
                existing.LockedUntil = null;
                existing.LastLoginAt = DateTime.UtcNow;
                existing.UpdatedAt = DateTime.UtcNow;
                
                await users.UpdateAsync(existing, ct);
                await users.SaveChangesAsync(ct);

                return Result<User>.Success(existing);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure("Authentication failed. Please try again.", ex);
            }
        }

        /// <summary>
        /// Registers a new user with the provided email and password.
        /// Validates uniqueness and securely hashes the password.
        /// </summary>
        /// <param name="email">The email address for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="ct">Cancellation token for async operation.</param>
        /// <returns>A result containing the registered user or error information.</returns>
        public async Task<Result<User>> RegisterAsync(string email, string password, CancellationToken ct = default)
        {
            try
            {
                // Check if user already exists
                var existing = await users.GetByEmailAsync(email, ct);
                if (existing != null)
                {
                    return Result<User>.Failure($"User with email '{email}' already exists.");
                }

                // Hash password with work factor
                var hash = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
                
                var user = new User 
                { 
                    Email = email.ToLowerInvariant(), 
                    PasswordHash = hash,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                
                await users.AddAsync(user, ct);
                await users.SaveChangesAsync(ct);
                
                return Result<User>.Success(user);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure("Registration failed. Please try again.", ex);
            }
        }
    }
}
