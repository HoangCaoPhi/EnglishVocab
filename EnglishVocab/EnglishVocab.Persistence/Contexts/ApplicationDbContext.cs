using EnglishVocab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
 
namespace EnglishVocab.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // Register entities
        public DbSet<Group> Groups { get; set; }

        public DbSet<Word> Words { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var entries = ChangeTracker
             .Entries()
             .Where(e => e.Entity is BaseEntity<int> && (
                     e.State == EntityState.Added
                     || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity<int>)entityEntry.Entity).ModifiedDate = DateTime.Now;
                ((BaseEntity<int>)entityEntry.Entity).ModifiedBy = "hcphi";

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity<int>)entityEntry.Entity).CreatedDate = DateTime.Now;
                    ((BaseEntity<int>)entityEntry.Entity).CreatedBy = "hcphi";
                }
            }

            return base.SaveChangesAsync();
        }
    }
}
