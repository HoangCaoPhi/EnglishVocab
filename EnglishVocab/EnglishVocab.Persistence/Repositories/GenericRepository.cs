using EnglishVocab.Domain.Entities;
using EnglishVocab.Domain.Interfaces;
using EnglishVocab.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EnglishVocab.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntityIdInt
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
             return await _dbSet.FindAsync(id);
        }

        public async Task<IReadOnlyList<TEntity>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            return await _dbSet
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> SaveAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> SaveListAsync(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            var numberRecordUpdate = await _dbContext.SaveChangesAsync();
            if(numberRecordUpdate > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateRangeAsync(List<TEntity> entity)
        {
            _dbSet.UpdateRange(entity);
            var result = await _dbContext.SaveChangesAsync();
            if(result > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            var result = await _dbContext.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await _dbSet
                 .ToListAsync();
        }
    }
}
