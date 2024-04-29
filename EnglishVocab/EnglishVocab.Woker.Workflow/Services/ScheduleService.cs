using Quartz.Impl;
using Quartz;
using EnglishVocab.Woker.Schedule.Provider;
using Quartz.Logging;
using EnglishVocab.Domain.Message;

namespace EnglishVocab.Woker.Schedule.Services
{
    public class ScheduleService
    {
        public async Task Run(DailyRemindGroup config)
        {
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

            // Grab the Scheduler instance from the Factory
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            // and start it off
            await scheduler.Start();

            // define the job and tie it to our HelloJob class
            var indentiyJob = $"Job_{config.IndentiyId}";
            IJobDetail job = JobBuilder.Create<NotificationJob>()
                .WithIdentity(indentiyJob, "email-sent")
                .UsingJobData(nameof(config.Id), config.Id)
                .UsingJobData(nameof(config.ActionRun), config.ActionRun.ToString())
                .UsingJobData(nameof(config.IndentiyId), config.IndentiyId)
                .UsingJobData(nameof(config.Status), config.Status.ToString())
                .Build();

            // Trigger the job to run now, and then repeat every 10 seconds
            var valid = CronExpression.IsValidExpression(config.CronSchedule);

            var indentiyTrigger = $"Trigger_{config.IndentiyId}";
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(indentiyTrigger, "email-sent")
                .StartNow()
                .WithCronSchedule(config.CronSchedule)
                .Build();

            // Tell Quartz to schedule the job using our trigger
            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
