using HiremeAuthGate.BusinessModel.BaseViewModel;
using HiremeAuthGate.BusinessModel.Results;
using HiremeAuthGate.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HiremeAuthGate.Services.Services
{
    /// <summary>
    /// Service implementation for authentication operations.
    /// Handles user authentication, registration, and account security features.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Initializes a new instance of the AuthService.
        /// </summary>
        /// <param name="users">The user repository dependency.</param>
        /// <param name="configuration">The configuration dependency.</param>
        /// <param name="logger">The logger dependency.</param>
        public AuthService(IUserRepository users, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _users = users;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Gets the maximum number of login attempts from configuration.
        /// </summary>
        private int MaxLoginAttempts => _configuration.GetValue<int>("Security:MaxLoginAttempts", 5);
        
        /// <summary>
        /// Gets the lockout duration in minutes from configuration.
        /// </summary>
        private int LockoutDurationMinutes => _configuration.GetValue<int>("Security:LockoutDurationMinutes", 15);

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
            _logger.LogInformation("Authentication attempt for email: {Email}", email);
            
            try
            {
                var existing = await _users.GetByEmailAsync(email, ct);
                if (existing is null)
                {
                    _logger.LogWarning("Authentication failed: User not found for email: {Email}", email);
                    return Result<User>.Failure("Invalid email or password.");
                }

                // Check if account is locked
                if (existing.LockedUntil.HasValue && existing.LockedUntil.Value > DateTime.UtcNow)
                {
                    var remainingTime = existing.LockedUntil.Value - DateTime.UtcNow;
                    _logger.LogWarning("Authentication failed: Account locked for email: {Email}, Remaining time: {RemainingMinutes} minutes", 
                        email, remainingTime.Minutes);
                    return Result<User>.Failure($"Account is locked. Please try again in {remainingTime.Minutes} minutes.");
                }

                // Check if account is active
                if (!existing.IsActive)
                {
                    _logger.LogWarning("Authentication failed: Account deactivated for email: {Email}", email);
                    return Result<User>.Failure("Account is deactivated. Please contact support.");
                }

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(password, existing.PasswordHash))
                {
                    // Increment login attempts
                    existing.LoginAttempts++;
                    
                    _logger.LogWarning("Authentication failed: Invalid password for email: {Email}, Attempt: {LoginAttempts}/{MaxAttempts}", 
                        email, existing.LoginAttempts, MaxLoginAttempts);
                    
                    // Lock account if max attempts reached
                    if (existing.LoginAttempts >= MaxLoginAttempts)
                    {
                        existing.LockedUntil = DateTime.UtcNow.AddMinutes(LockoutDurationMinutes);
                        existing.LoginAttempts = 0;
                        _logger.LogWarning("Account locked due to max login attempts for email: {Email}, Lockout until: {LockoutUntil}", 
                            email, existing.LockedUntil);
                    }
                    
                    existing.UpdatedAt = DateTime.UtcNow;
                    await _users.UpdateAsync(existing, ct);
                    await _users.SaveChangesAsync(ct);
                    
                    return Result<User>.Failure("Invalid email or password.");
                }

                // Reset login attempts on successful login
                existing.LoginAttempts = 0;
                existing.LockedUntil = null;
                existing.LastLoginAt = DateTime.UtcNow;
                existing.UpdatedAt = DateTime.UtcNow;
                
                await _users.UpdateAsync(existing, ct);
                await _users.SaveChangesAsync(ct);

                _logger.LogInformation("Authentication successful for email: {Email}", email);
                return Result<User>.Success(existing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Authentication failed with exception for email: {Email}", email);
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
            _logger.LogInformation("Registration attempt for email: {Email}", email);
            
            try
            {
                // Check if user already exists
                var existing = await _users.GetByEmailAsync(email, ct);
                if (existing != null)
                {
                    _logger.LogWarning("Registration failed: User already exists for email: {Email}", email);
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
                
                await _users.AddAsync(user, ct);
                await _users.SaveChangesAsync(ct);
                
                _logger.LogInformation("Registration successful for email: {Email}, UserId: {UserId}", email, user.Id);
                return Result<User>.Success(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed with exception for email: {Email}", email);
                return Result<User>.Failure("Registration failed. Please try again.", ex);
            }
        }
    }
}
