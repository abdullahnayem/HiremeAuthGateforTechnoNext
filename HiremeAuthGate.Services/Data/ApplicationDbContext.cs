using HiremeAuthGate.BusinessModel.BaseViewModel;
using Microsoft.EntityFrameworkCore;

namespace HiremeAuthGate.Services.Data
{
    /// <summary>
    /// Entity Framework Core database context for the authentication system.
    /// Manages database connections and entity configurations.
    /// </summary>
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        /// <summary>
        /// Gets the Users DbSet for database operations on user entities.
        /// </summary>
        public DbSet<User> Users => Set<User>();

        /// <summary>
        /// Configures the database model and entity relationships.
        /// Sets up table names, keys, indexes, and default values.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure the database model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("Users");
                b.HasKey(x => x.Id);
                b.Property(x => x.Email).IsRequired().HasMaxLength(256);
                b.HasIndex(x => x.Email).IsUnique();
                b.Property(x => x.PasswordHash).IsRequired();
                b.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                b.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
                b.Property(x => x.IsActive).HasDefaultValue(true);
                b.Property(x => x.LoginAttempts).HasDefaultValue(0);
            });
        }
    }
}
