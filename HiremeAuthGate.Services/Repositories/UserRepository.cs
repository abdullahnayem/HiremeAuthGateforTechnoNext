using HiremeAuthGate.BusinessModel.BaseViewModel;
using HiremeAuthGate.Services.Data;
using HiremeAuthGate.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HiremeAuthGate.Services.Repositories
{
    /// <summary>
    /// Repository implementation for user data access operations.
    /// Provides concrete implementation of user repository interface using Entity Framework Core.
    /// </summary>
    public class UserRepository(ApplicationDbContext db) : IUserRepository
    {
        /// <summary>
        /// Retrieves a user by their email address.
        /// Uses case-insensitive email comparison and AsNoTracking for read-only operations.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <param name="ct">Cancellation token for async operation.</param>
        /// <returns>The user with the specified email, or null if not found.</returns>
        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
            => await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), ct);

        /// <summary>
        /// Adds a new user to the database context.
        /// </summary>
        /// <param name="user">The user entity to add.</param>
        /// <param name="ct">Cancellation token for async operation.</param>
        public async Task AddAsync(User user, CancellationToken ct = default)
            => await db.Users.AddAsync(user, ct);

        /// <summary>
        /// Marks an existing user as modified in the database context.
        /// </summary>
        /// <param name="user">The user entity to update.</param>
        /// <param name="ct">Cancellation token for async operation.</param>
        public Task UpdateAsync(User user, CancellationToken ct = default)
        {
            db.Users.Update(user);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Persists all pending changes to the database.
        /// </summary>
        /// <param name="ct">Cancellation token for async operation.</param>
        public Task SaveChangesAsync(CancellationToken ct = default)
            => db.SaveChangesAsync(ct);
    }
}
