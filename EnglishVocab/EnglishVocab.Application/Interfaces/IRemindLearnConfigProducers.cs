using EnglishVocab.Domain.Entities;

namespace EnglishVocab.Application.Interfaces
{
    public interface IRemindLearnConfigProducers
    {
        Task SendReminderConfig(Group group);
    }
}
