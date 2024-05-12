using EnglishVocab.Domain.Enums;

namespace EnglishVocab.Domain.Entities.Schedule
{
    public class ScheduleExecution : BaseEntity<int>
    {
        public ActionRun ActionRun { get; set; }

        public string CronSchedule { get; set; }

        public string ScheduleDate { get; set; }

        public string EndDate { get; set; }

        public string IndentiyId { get; set; }

        public string Interval { get; set; }

        public Status Status { get; set; }
    }
}
