using EnglishVocab.Application.Interfaces;
using EnglishVocab.Domain.Entities;
using EnglishVocab.Domain.Enums;
using EnglishVocab.Domain.Message;
using MassTransit;

namespace EnglishVocab.Application.Producers
{
    public class RemindLearnConfigProducers : IRemindLearnConfigProducers
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public RemindLearnConfigProducers(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task SendReminderConfig(Group group)
        { 
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:schedule-config"));

            await endpoint.Send<DailyRemindGroup>(new {  
                group.Id,
                ActionRun = ActionRun.SendMail,
                CronSchedule = group.DailyHourlyReminders,
                IndentiyId = $"{group.Id}_{group.GroupName}",
            });             
        }
    }
}
