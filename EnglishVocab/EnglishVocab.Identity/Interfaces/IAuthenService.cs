using EnglishVocab.Identity.Dtos.Requests.Authen;
using EnglishVocab.Identity.Dtos.Responses.Authen;
using EnglishVocab.Shared.Wrappers;
using Microsoft.AspNetCore.Identity;

namespace EnglishVocab.Identity.Interfaces;

public interface IAuthenService
{
    Task<Response<LoginReponse>> LoginAsync(LoginRequest request, string ipAddress);
    Task<Response<IdentityResult>> RegisterAsync(RegisterRequest request, string origin);
}
