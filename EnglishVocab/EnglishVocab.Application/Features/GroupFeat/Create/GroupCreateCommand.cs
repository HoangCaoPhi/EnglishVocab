using EnglishVocab.Application.Interfaces;
using EnglishVocab.Domain.Entities;
using EnglishVocab.Domain.Interfaces;
using EnglishVocab.Shared.Wrappers;
using MediatR;

namespace EnglishVocab.Application.Features.GroupFeat.Create
{
    public class GroupCreateCommand : IRequest<ServiceResponse<int>>
    {
        public required Group Group { get; set; }

        public class GroupCreateCommandHandler(IGroupRepo groupRepo,
            IRemindLearnConfigProducers remind) : IRequestHandler<GroupCreateCommand, ServiceResponse<int>>
        {
            private readonly IGroupRepo _groupRepo = groupRepo;
            private readonly IRemindLearnConfigProducers _remind = remind;

            public async Task<ServiceResponse<int>> Handle(GroupCreateCommand request, CancellationToken cancellationToken)
            {
                var id = await _groupRepo.CreateAsync(request.Group);
                await _remind.SendReminderConfig(request.Group);

                return new ServiceResponse<int>(id);
            }
        }
    }
}
