using Asp.Versioning;
using EnglishVocab.Application.Models;
using EnglishVocab.Identity.Dtos.Requests.Authen;
using EnglishVocab.Identity.Interfaces;
using EnglishVocab.Shared.Wrappers;
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

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(LoginRequest request)
        {
            return Ok(await _authenService.LoginAsync(request, GenerateIPAddress()));
        }

        [HttpPost("logout")]
        public async Task<ServiceResponse> SignOut()
        {
            return null;
        }
    }
}
