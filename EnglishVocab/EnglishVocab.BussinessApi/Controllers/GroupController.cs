using Asp.Versioning;
using EnglishVocab.Application.Features.GroupFeat.Create;
using EnglishVocab.Application.Models;
using EnglishVocab.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EnglishVocab.BussinessApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/Group")]
    public class GroupController : Controller
    {
        private readonly IMediator _mediator;
        public GroupController(IMediator mediator)
        {
            _mediator = mediator;
        }
 
        [HttpPost("")]
        public async Task<ServiceResponse> SaveGroup(Group group)
        {
            var data = await _mediator.Send(new GroupCreateCommand()
            {
                Group = group
            });

            return new ServiceResponse(data);
        }
    }
}
