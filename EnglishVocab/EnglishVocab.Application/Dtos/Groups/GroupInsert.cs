using EnglishVocab.Application.Dtos.Words;

namespace EnglishVocab.Application.Dtos.Groups;
public class GroupInsert
{
    public string GroupName { get; set; }

    public string? Description { get; set; }

    public string? DailyHourlyReminders { get; set; }

    public List<WordInsert> Words { get; set; }
}
