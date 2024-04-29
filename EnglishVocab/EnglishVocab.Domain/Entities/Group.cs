using System.ComponentModel.DataAnnotations;

namespace EnglishVocab.Domain.Entities
{
    public class Group : BaseEntityIdInt
    {
        [MaxLength(500)]
        public string GroupName { get; set; }

        public string? Description { get; set; }

        public string? DailyHourlyReminders { get; set; }

        public ICollection<Word>? Words { get; set; }
    }
}
