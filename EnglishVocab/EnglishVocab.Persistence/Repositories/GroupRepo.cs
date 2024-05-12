using EnglishVocab.Domain.Entities;
using EnglishVocab.Domain.Interfaces;
using EnglishVocab.Persistence.Contexts;

namespace EnglishVocab.Persistence.Repositories;
public class GroupRepo(ApplicationDbContext dbContext) : GenericRepository<Group>(dbContext), IGroupRepo
{
    public async Task<int> CreateAsync(Group group)
    {
        return await SaveAsync(group);
    }
}