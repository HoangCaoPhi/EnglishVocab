using EnglishVocab.Application.Dtos.Words;

namespace EnglishVocab.Application.Dtos.Groups;
public class GroupDetail
{
    public int Id { get; set; }
    public string GroupName { get; set; }

    public string? Description { get; set; }

    public string? DailyHourlyReminders { get; set; }

    public List<WordDto> Words { get; set; }
}
