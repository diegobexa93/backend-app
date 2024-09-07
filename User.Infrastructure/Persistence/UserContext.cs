using BaseShare.Common.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetDevPack.Security.Jwt.Core.Model;
using NetDevPack.Security.Jwt.Store.EntityFrameworkCore;
using User.Domain.Entities;
using User.Infrastructure.EntityConfigurations;

namespace User.Infrastructure.Persistence
{
    public class UserContext(DbContextOptions<UserContext> options) : DbContext(options), ISecurityKeyContext
    {
        public DbSet<UserObj> User { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<KeyMaterial> SecurityKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PersonEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new KeyMaterialEntityTypeConfiguration());
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.GuidId = Guid.NewGuid();
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.CreatedBy = "Admin1";
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = "Admin2";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
