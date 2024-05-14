using EnglishVocab.Application.Interfaces.Repos;
using EnglishVocab.Domain.Entities;
using EnglishVocab.Persistence.Contexts;

namespace EnglishVocab.Persistence.Repositories;
public class WordRepo(ApplicationDbContext dbContext) : GenericRepository<Word>(dbContext), IWordRepo
{
    protected override void SetForeignKey(Word entity, int id)
    {
        entity.GroupId = id;
    }

    protected override void UpdateFieldEntity(Word existingEntity, Word entity)
    {
        existingEntity.Vocabulary = entity.Vocabulary;
        existingEntity.Pronunciation = entity.Pronunciation;
    }
}
