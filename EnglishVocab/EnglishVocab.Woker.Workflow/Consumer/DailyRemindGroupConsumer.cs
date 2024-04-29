using EnglishVocab.Domain.Message;
using EnglishVocab.Woker.Schedule.Services;
using MassTransit;

namespace EnglishVocab.Woker.Schedule.Consumer
{
    public class DailyRemindGroupConsumer : IConsumer<DailyRemindGroup>
    {
        public async Task Consume(ConsumeContext<DailyRemindGroup> context)
        {
            var data = context.Message;
            var service = new ScheduleService();
            await service.Run(data);
        }
    }
}
