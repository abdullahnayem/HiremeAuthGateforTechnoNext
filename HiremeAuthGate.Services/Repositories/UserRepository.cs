using Dapper; // Add this
using HiremeAuthGate.BusinessModel.BaseViewModel;
using HiremeAuthGate.Services.Data;
using HiremeAuthGate.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // Add this
using Microsoft.Extensions.Logging;

namespace HiremeAuthGate.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<UserRepository> _logger;
        private readonly string _connectionString;

        public UserRepository(ApplicationDbContext db, ILogger<UserRepository> logger, IConfiguration configuration)
        {
            _db = db;
            _logger = logger;
            _connectionString = configuration.GetConnectionString("Default");
        }

        /// <summary>
        /// Retrieves a user by their email address.
        /// Uses case-insensitive email comparison and AsNoTracking for read-only operations.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <param name="ct">Cancellation token for async operation.</param>
        /// <returns>The user with the specified email, or null if not found.</returns>
        // This method will use Dapper for better performance
        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            try
            {
                _logger.LogDebug("Retrieving user by email using Dapper: {Email}", email);

                // Use Dapper for this query
                const string sql = @"
                    SELECT Id, Email, PasswordHash, CreatedAt, LastLoginAt, 
                           IsActive, LoginAttempts, LockedUntil, UpdatedAt, UpdatedBy
                    FROM Users 
                    WHERE Email = @Email";

                using var connection = new SqlConnection(_connectionString);
                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email.ToLowerInvariant() });

                if (user == null)
                {
                    _logger.LogDebug("User not found for email: {Email}", email);
                }
                else
                {
                    _logger.LogDebug("User found for email: {Email}, UserId: {UserId}", email, user.Id);
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by email: {Email}", email);
                throw;
            }
        }

        /// <summary>
        /// Adds a new user to the database context.
        /// </summary>
        /// <param name="user">The user entity to add.</param>
        /// <param name="ct">Cancellation token for async operation.</param>
        // Keep Entity Framework for other methods
        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            try
            {
                _logger.LogDebug("Adding new user: {Email}, UserId: {UserId}", user.Email, user.Id);
                await _db.Users.AddAsync(user, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user: {Email}, UserId: {UserId}", user.Email, user.Id);
                throw;
            }
        }

        /// <summary>
        /// Marks an existing user as modified in the database context.
        /// </summary>
        /// <param name="user">The user entity to update.</param>
        /// <param name="ct">Cancellation token for async operation.</param>
        public Task UpdateAsync(User user, CancellationToken ct = default)
        {
            try
            {
                _logger.LogDebug("Updating user: {Email}, UserId: {UserId}", user.Email, user.Id);
                _db.Users.Update(user);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {Email}, UserId: {UserId}", user.Email, user.Id);
                throw;
            }
        }

        /// <summary>
        /// Persists all pending changes to the database.
        /// </summary>
        /// <param name="ct">Cancellation token for async operation.</param>
        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            try
            {
                _logger.LogDebug("Saving changes to database");
                await _db.SaveChangesAsync(ct);
                _logger.LogDebug("Changes saved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving changes to database");
                throw;
            }
        }
    }
}
