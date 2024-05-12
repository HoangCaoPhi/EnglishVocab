namespace EnglishVocab.Domain.Interfaces;
public interface IUnitOfWork
{
    void SaveAnnotatedGraph(object rootEntity);
}
