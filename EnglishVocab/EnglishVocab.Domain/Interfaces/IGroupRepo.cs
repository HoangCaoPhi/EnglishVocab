using EnglishVocab.Domain.Entities;

namespace EnglishVocab.Domain.Interfaces
{
    public interface IGroupRepo : IGenericRepository<Group>
    {
        Task<int> CreateAsync(Group group);
    }
}
