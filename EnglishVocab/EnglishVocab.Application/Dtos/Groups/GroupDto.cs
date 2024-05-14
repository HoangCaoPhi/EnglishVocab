namespace EnglishVocab.Application.Dtos.Groups;
public class GroupDto
{
    public int Id { get; set; }

    public string GroupName { get; set; }

    public string? Description { get; set; }

    public string? DailyHourlyReminders { get; set; }
}
