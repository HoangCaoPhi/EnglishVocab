using EnglishVocab.Application.Dtos.Groups;
using EnglishVocab.Domain.Entities;
using EnglishVocab.Domain.Interfaces;
using EnglishVocab.Shared.Wrappers;
using MediatR;
using Newtonsoft.Json;

namespace EnglishVocab.Application.Features.Groups.Update;
public class UpdateGroupCommandRequest : IRequest<ServiceResponse<int>>
{
    public GroupUpdate GroupUpdate { get; set; }

    public class UpdateGroupCommandRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateGroupCommandRequest, ServiceResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ServiceResponse<int>> Handle(UpdateGroupCommandRequest request, CancellationToken cancellationToken)
        {
            var groupOld = await _unitOfWork.GroupRepo.GetGroupById(request.GroupUpdate.Id);
            var groupNew = JsonConvert.DeserializeObject<Group>(JsonConvert.SerializeObject(request.GroupUpdate));
            await _unitOfWork.UpdateEntityComplex(groupOld, groupNew);

            var result = _unitOfWork.Complete();
            return new ServiceResponse<int>(result);
        }
    }
}
