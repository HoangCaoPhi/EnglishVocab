using EnglishVocab.Application.Enums;
using EnglishVocab.Domain.Entities;
using EnglishVocab.Domain.Interfaces;
using EnglishVocab.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EnglishVocab.Persistence.Repositories;
public class UnitOfWork(ApplicationDbContext applicationDbContext) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = applicationDbContext;

    public void SaveAnnotatedGraph(object rootEntity)
    {
        _context.ChangeTracker.TrackGraph(
            rootEntity,
            n =>
            {
                var entity = (BaseEntity<int>)n.Entry.Entity;
      
                var repo = GetRepoFactory(entity.GetEntityType());

                if (entity.ModelState == ModelState.Insert)
                {
                    n.Entry.State = EntityState.Added;
                }
                else if (entity.ModelState == ModelState.Update)
                {
                    var existingEntity = repo.GetByID(entity.Id)?.GetAwaiter().GetResult();
                    if (existingEntity is not null)
                    {
                        repo.UpdateFieldEntityWrap(existingEntity, entity);
                        _context.Entry(entity).CurrentValues.SetValues(existingEntity);
                        n.Entry.State = EntityState.Modified;
                    }

                }
                else if (entity.ModelState == ModelState.DeleteSoft)
                {
                    var existingEntityDelete = repo.GetByID(entity.Id)?.GetAwaiter().GetResult();
                    if (existingEntityDelete is not null)
                    {
                        existingEntityDelete.IsDeleted = true;
                        _context.Entry(entity).CurrentValues.SetValues(existingEntityDelete);
                        n.Entry.State = EntityState.Modified;
                    }
                }
                else
                {
                    n.Entry.State = EntityState.Unchanged;
                }
            });
    }

    private IGenericRepositoryWrap GetRepoFactory(Type type)
    {
        throw new NotImplementedException();
    }
}
