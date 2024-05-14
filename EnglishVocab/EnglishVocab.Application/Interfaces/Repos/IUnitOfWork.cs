using EnglishVocab.Application.Interfaces.Repos;
using EnglishVocab.Domain.Entities;

namespace EnglishVocab.Domain.Interfaces;
public interface IUnitOfWork
{
    void SaveAnnotatedGraph(object rootEntity);

    Task UpdateEntityComplex(BaseEntity<int> oldEntity, BaseEntity<int> newEntity);

    IGroupRepo GroupRepo { get; }

    IWordRepo WordRepo { get; }

    int Complete();
}
