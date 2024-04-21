using Asp.Versioning;
using EnglishVocab.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnglishVocab.BussinessApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/[controller]")]
    public class UserController : Controller
    {
        [HttpPost("sign-up")]
        public async Task<ServiceResponse> SignUp()
        {
            return null;
        }

        [HttpPost("sign-in")]
        public async Task<ServiceResponse> SignIn()
        {
            return null;
        }

        [HttpPost("sign-out")]
        public async Task<ServiceResponse> SignOut()
        {
            return null;
        }
    }
}
