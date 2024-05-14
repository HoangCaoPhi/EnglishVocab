using EnglishVocab.Application.Common.Utils;
using EnglishVocab.Application.Dtos.Groups;
using EnglishVocab.Application.Interfaces;
using EnglishVocab.Domain.Entities;
using EnglishVocab.Domain.Interfaces;
using EnglishVocab.Shared.Wrappers;
using MediatR;

namespace EnglishVocab.Application.Features.GroupFeat.Create
{
    public class GroupCreateCommandRequest : IRequest<ServiceResponse<int>>
    {
        public required GroupInsert Group { get; set; }

        public class GroupCreateCommandRequestHandler(IGroupRepo groupRepo,
            IRemindLearnConfigProducers remind) : IRequestHandler<GroupCreateCommandRequest, ServiceResponse<int>>
        {
            private readonly IGroupRepo _groupRepo = groupRepo;
            private readonly IRemindLearnConfigProducers _remind = remind;

            public async Task<ServiceResponse<int>> Handle(GroupCreateCommandRequest request, CancellationToken cancellationToken)
            {
                var group = ConvertUtils.Map<GroupInsert, Group>(request.Group);
                var id = await _groupRepo.CreateAsync(group);

                //await _remind.SendReminderConfig(request.Group);

                return new ServiceResponse<int>(id);
            }
        }
    }
}
