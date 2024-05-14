using Asp.Versioning;
using EnglishVocab.Application.Dtos.Groups;
using EnglishVocab.Application.Features.GroupFeat.Create;
using EnglishVocab.Application.Features.Groups.Query;
using EnglishVocab.Application.Features.Groups.Update;
using Microsoft.AspNetCore.Mvc;

namespace EnglishVocab.BussinessApi.Controllers
{
    //[Authorize]
    [ApiVersion("1.0")]
    public class GroupController() : BaseController
    {        
        [HttpPost("")]
        public async Task<IActionResult> SaveGroup(GroupInsert group)
        {
            var data = await Mediator.Send(new GroupCreateCommandRequest { Group = group });
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await Mediator.Send(new GetGroupByIdQueryRequest { Id = id });
            return Ok(data);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateGroup(GroupUpdate group)
        {
            var data = await Mediator.Send(new UpdateGroupCommandRequest { GroupUpdate = group });
            return Ok(data);
        }

    }
}
