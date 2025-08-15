using HiremeAuthGate.BusinessModel.BaseViewModel;
using HiremeAuthGate.Services.Data;
using HiremeAuthGate.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HiremeAuthGate.Services.Repositories
{
    public class UserRepository(ApplicationDbContext db) : IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
            => await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), ct);

        public async Task AddAsync(User user, CancellationToken ct = default)
            => await db.Users.AddAsync(user, ct);

        public Task UpdateAsync(User user, CancellationToken ct = default)
        {
            db.Users.Update(user);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
            => db.SaveChangesAsync(ct);
    }
}
