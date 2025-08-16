using HiremeAuthGate.BusinessModel.BaseViewModel;
using HiremeAuthGate.BusinessModel.Results;

namespace HiremeAuthGate.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<User>> AuthenticateAsync(string email, string password, CancellationToken ct = default);

        Task<Result<User>> RegisterAsync(string email, string password, CancellationToken ct = default);
    }
}
