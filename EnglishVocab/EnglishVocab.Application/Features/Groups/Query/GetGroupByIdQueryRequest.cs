using EnglishVocab.Application.Dtos.Groups;
using EnglishVocab.Domain.Interfaces;
using EnglishVocab.Shared.Wrappers;
using MediatR;

namespace EnglishVocab.Application.Features.Groups.Query;
public class GetGroupByIdQueryRequest : IRequest<ServiceResponse<GroupDetail>>
{
    public int Id { get; set; }
    public class GetGroupByIdQueryRequestHandler(IGroupRepo groupRepo) : IRequestHandler<GetGroupByIdQueryRequest, ServiceResponse<GroupDetail>>
    {
        private readonly IGroupRepo _groupRepo = groupRepo;

        public async Task<ServiceResponse<GroupDetail>> Handle(GetGroupByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var group = await _groupRepo.GetGroupDetailById(request.Id);            
            return new ServiceResponse<GroupDetail>(group);
        }
    }
}
