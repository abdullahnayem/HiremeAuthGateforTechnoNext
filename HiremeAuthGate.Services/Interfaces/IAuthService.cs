using HiremeAuthGate.BusinessModel.BaseViewModel;
using HiremeAuthGate.BusinessModel.Results;

namespace HiremeAuthGate.Services.Interfaces
{
    /// <summary>
    /// Service interface for authentication operations.
    /// Provides methods for user authentication and registration.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user with the provided email and password.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The password to authenticate.</param>
        /// <param name="ct">Cancellation token for async operation.</param>
        /// <returns>A result containing the authenticated user or error information.</returns>
        Task<Result<User>> AuthenticateAsync(string email, string password, CancellationToken ct = default);
        
        /// <summary>
        /// Registers a new user with the provided email and password.
        /// </summary>
        /// <param name="email">The email address for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="ct">Cancellation token for async operation.</param>
        /// <returns>A result containing the registered user or error information.</returns>
        Task<Result<User>> RegisterAsync(string email, string password, CancellationToken ct = default);
    }
}
