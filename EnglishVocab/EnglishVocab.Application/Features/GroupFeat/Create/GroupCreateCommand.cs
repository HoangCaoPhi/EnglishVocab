using EnglishVocab.Application.Interfaces;
using EnglishVocab.Domain.Entities;
using EnglishVocab.Domain.Interfaces;
using MediatR;

namespace EnglishVocab.Application.Features.GroupFeat.Create
{
    public class GroupCreateCommand : IRequest<int>
    {
        public Group Group { get; set; }

        public class GroupCreateCommandHandler : IRequestHandler<GroupCreateCommand, int>
        {
            private readonly IGroupRepo _groupRepo;
            private readonly IRemindLearnConfigProducers _remind;

            public GroupCreateCommandHandler(IGroupRepo groupRepo,
                IRemindLearnConfigProducers remind)
            {
                _groupRepo = groupRepo;
                _remind = remind;
            }
            public async Task<int> Handle(GroupCreateCommand request, CancellationToken cancellationToken)
            {
                var id = await _groupRepo.CreateAsync(request.Group);
                await _remind.SendReminderConfig(request.Group);

                return id;
            }
        }
    }
}
