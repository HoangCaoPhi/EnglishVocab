using Asp.Versioning;
using EnglishVocab.Application.Features.GroupFeat.Create;
using EnglishVocab.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishVocab.BussinessApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    public class GroupController() : BaseController
    {        
        [HttpPost("")]
        public async Task<IActionResult> SaveGroup(Group group)
        {
            var data = await Mediator.Send(new GroupCreateCommand { Group = group });
            return Ok(data);
        }
    }
}
