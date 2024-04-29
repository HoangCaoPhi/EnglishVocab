using EnglishVocab.Domain.Entities;
using EnglishVocab.Domain.Interfaces;
using EnglishVocab.Persistence.Contexts;

namespace EnglishVocab.Persistence.Repositories
{
    public class GroupRepo : GenericRepository<Group>, IGroupRepo
    {
        public GroupRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> CreateAsync(Group group)
        {
            return await SaveAsync(group);
        }
    }
}
