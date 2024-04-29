using EnglishVocab.Domain.Entities;
using EnglishVocab.Domain.Entities.Schedule;
using Microsoft.EntityFrameworkCore;

namespace EnglishVocab.Persistence.Contexts
{
    public class ScheduleContext : DbContext
    {
        public ScheduleContext(DbContextOptions<ScheduleContext> options) : base(options)
        {

        }

        public DbSet<ScheduleExecution> Schedules { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var entries = ChangeTracker
             .Entries()
             .Where(e => e.Entity is BaseEntityIdInt && (
                     e.State == EntityState.Added
                     || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntityIdInt)entityEntry.Entity).ModifiedDate = DateTime.Now;
                ((BaseEntityIdInt)entityEntry.Entity).ModifiedBy = "hcphi";

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntityIdInt)entityEntry.Entity).CreatedDate = DateTime.Now;
                    ((BaseEntityIdInt)entityEntry.Entity).CreatedBy = "hcphi";
                }
            }

            return base.SaveChangesAsync();
        }
    }
}
