using Asp.Versioning;
using EnglishVocab.Application.Models;
using EnglishVocab.Identity.Dtos.Requests.Authen;
using EnglishVocab.Identity.Interfaces;
using EnglishVocab.Identity.Utils;
using Microsoft.AspNetCore.Mvc;

namespace EnglishVocab.BussinessApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AuthenController(IAuthenService authenService) : Controller
    {
        private readonly IAuthenService _authenService = authenService;

        [HttpPost("register")]
        public async Task<IActionResult> SignUp(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _authenService.RegisterAsync(request, origin));
        }

 
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(LoginRequest request)
        {
            var ip = IdentityUtils.GenerateIPAddress(HttpContext);
            return Ok(await _authenService.LoginAsync(request, ip));
        }

        [HttpPost("logout")]
        public async Task<ServiceResponse> SignOut()
        {
            return null;
        }
    }
}
