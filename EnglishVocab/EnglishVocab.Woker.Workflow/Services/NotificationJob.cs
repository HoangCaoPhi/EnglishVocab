using EnglishVocab.Domain.Enums;
using Quartz;

namespace EnglishVocab.Woker.Schedule.Services
{
    public class NotificationJob : IJob
    {
        public int Id { get; set; }

        public ActionRun ActionRun { get; set; }

        public string IndentiyId { get; set; }

        public Status Status { get; set; }


        public async Task Execute(IJobExecutionContext context)
        {
            JobKey key = context.JobDetail.Key;
            JobDataMap dataMap = context.MergedJobDataMap;

            await Console.Out.WriteLineAsync($"NotificationJob is executing and key : " +
                $"{key} {Id} and {IndentiyId} run at {DateTime.Now}.");
        }
    }
}
