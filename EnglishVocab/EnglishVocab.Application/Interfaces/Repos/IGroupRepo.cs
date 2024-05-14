using EnglishVocab.Application.Dtos.Groups;
using EnglishVocab.Domain.Entities;

namespace EnglishVocab.Domain.Interfaces
{
    public interface IGroupRepo : IGenericRepository<Group>
    {
        Task<int> CreateAsync(Group group);

        Task<GroupDetail> GetGroupDetailById(int id);

        Task<Group> GetGroupById(int id);
    }
}
