using EnglishVocab.Application.Dtos.Groups;
using EnglishVocab.Application.Dtos.Words;
using EnglishVocab.Domain.Entities;
using EnglishVocab.Domain.Interfaces;
using EnglishVocab.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EnglishVocab.Persistence.Repositories;
public class GroupRepo(ApplicationDbContext dbContext) : GenericRepository<Group>(dbContext), IGroupRepo
{
    public async Task<int> CreateAsync(Group group)
    {
        return await SaveAsync(group);
    }

    public async Task<GroupDetail> GetGroupDetailById(int id)
    {
        var group = dbContext.Set<Group>()
                    .Where(x => x.Id == id)
                    .Select(x => new GroupDetail
                    {
                        Id = x.Id,
                        GroupName = x.GroupName,
                        DailyHourlyReminders = x.DailyHourlyReminders,
                        Description = x.Description,
                        Words = x.Words.Select(y => new WordDto 
                        { 
                            Id = y.Id,
                            Pronunciation = y.Pronunciation,
                            Vocabulary  = y.Vocabulary
                        }).ToList()
                    })
                    .FirstOrDefault();

        return group;
    }

    public async Task<Group> GetGroupById(int id)
    {
        var group = await dbContext.Set<Group>()
                    .Include(x => x.Words)
                    .FirstOrDefaultAsync(x => x.Id == id);

        return group;
    }

    protected override void UpdateFieldEntity(Group existingEntity, Group entity)
    {
        existingEntity.GroupName = entity.GroupName;
        existingEntity.Description = entity.Description;
        existingEntity.DailyHourlyReminders  = entity.DailyHourlyReminders;
    }

}