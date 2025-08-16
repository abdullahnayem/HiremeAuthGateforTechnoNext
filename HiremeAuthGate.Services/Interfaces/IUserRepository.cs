using HiremeAuthGate.BusinessModel.BaseViewModel;

namespace HiremeAuthGate.Services.Interfaces
{
    /// <summary>
    /// Repository interface for user data access operations.
    /// Provides methods for retrieving, adding, and updating user entities.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <param name="ct">Cancellation token for async operation.</param>
        /// <returns>The user with the specified email, or null if not found.</returns>
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        
        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="user">The user entity to add.</param>
        /// <param name="ct">Cancellation token for async operation.</param>
        Task AddAsync(User user, CancellationToken ct = default);
        
        /// <summary>
        /// Updates an existing user in the repository.
        /// </summary>
        /// <param name="user">The user entity to update.</param>
        /// <param name="ct">Cancellation token for async operation.</param>
        Task UpdateAsync(User user, CancellationToken ct = default);
        
        /// <summary>
        /// Saves all pending changes to the database.
        /// </summary>
        /// <param name="ct">Cancellation token for async operation.</param>
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
