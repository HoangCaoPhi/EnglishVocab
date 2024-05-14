using EnglishVocab.Domain.Entities;

namespace EnglishVocab.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> : IGenericRepositoryWrap where TEntity : BaseEntity<int>
    {
        Task<TEntity> GetByIdAsync(int id);

        Task<IReadOnlyList<TEntity>> GetPagedReponseAsync(int pageNumber, int pageSize);

        Task<int> SaveAsync(TEntity entity);

        Task<bool> SaveListAsync(List<TEntity> entities);

        Task<bool> UpdateRangeAsync(List<TEntity> entity); 

        Task<bool> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(TEntity entity);

        Task<IReadOnlyList<TEntity>> GetAllAsync();
    }
}
