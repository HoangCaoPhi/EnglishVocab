﻿using EnglishVocab.Domain.Entities;
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
