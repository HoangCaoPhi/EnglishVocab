using EnglishVocab.Domain.Entities;

namespace EnglishVocab.Domain.Interfaces;
public interface IGenericRepositoryWrap
{
    Task<BaseEntity<int>> GetByID(int id);
    void UpdateFieldEntityWrap(BaseEntity<int> existingEntity, BaseEntity<int> entity);
}
