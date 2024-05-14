using EnglishVocab.Application.Enums;
using EnglishVocab.Application.Interfaces.Repos;
using EnglishVocab.Domain.Entities;
using EnglishVocab.Domain.Interfaces;
using EnglishVocab.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;

namespace EnglishVocab.Persistence.Repositories;
public class UnitOfWork(ApplicationDbContext applicationDbContext, 
                        IServiceProvider serviceProvider) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = applicationDbContext;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IGroupRepo GroupRepo => _serviceProvider.GetService<IGroupRepo>();

    public IWordRepo WordRepo => _serviceProvider.GetService<IWordRepo>();

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
        if (type == typeof(Group)) return GroupRepo;
        if (type == typeof(Word)) return WordRepo;

        throw new InvalidOperationException($"No repository found for type {type.GetType().Name}");
    }

    public async Task UpdateEntityComplex(BaseEntity<int> oldEntity, BaseEntity<int> newEntity)
    {
        if (oldEntity is null) ArgumentNullException.ThrowIfNull(oldEntity);
        if (newEntity is null) ArgumentNullException.ThrowIfNull(newEntity);

        var repoEntity = GetRepoFactory(oldEntity.GetEntityType());

        repoEntity.UpdateFieldEntityWrap(oldEntity, newEntity);
        repoEntity.ChangeStateUpdateWrap(oldEntity);

        foreach (var property in oldEntity.GetEntityType().GetProperties())
        {
            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
            {
                var oldEntities = property.GetValue(oldEntity) as IEnumerable<BaseEntity<int>>;
                var newEntities = property.GetValue(newEntity) as IEnumerable<BaseEntity<int>>;

                oldEntities ??= [];
                newEntities ??= [];

                // Xác định các phần tử bị xóa
                var deletedItems = oldEntities.Where(o => !newEntities.Any(n => n.Id == o.Id)).ToList();
                foreach (var entity in deletedItems)
                {
                    var repo = GetRepoFactory(entity.GetType());
                    entity.IsDeleted = true;
                    repo.ChangeStateUpdateWrap(entity);
                }

                // Xác định các phần tử được thêm mới
                var addedItems = newEntities.Where(n => !oldEntities.Any(o => o.Id == n.Id)).ToList();
                foreach (var entity in addedItems)
                {
                    var repo = GetRepoFactory(entity.GetType());
                    repo.SetForeignKeyWrap(entity, oldEntity.Id);
                    await repo.AddEntity(entity);
                }

                // Xác định và cập nhật các phần tử cần cập nhật
                var updateItems = newEntities.Where(n => oldEntities.Any(o => o.Id == n.Id && n.Id > 0)).ToList();
                foreach (var newItem in updateItems)
                {
                    var oldItem = oldEntities.First(o => o.Id == newItem.Id);
                    await UpdateEntityComplex(oldItem, newItem); // Gọi đệ quy để cập nhật các thuộc tính lồng nhau
                }
            }
        }
    }

    public int Complete()
    {
        return _context.SaveChanges();
    }
}
