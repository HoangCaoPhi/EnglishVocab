using EnglishVocab.Identity.Dtos;
using EnglishVocab.Identity.Dtos.Requests.Authen;
using EnglishVocab.Identity.Dtos.Responses.Authen;
using EnglishVocab.Shared.Wrappers;
using Microsoft.AspNetCore.Identity;

namespace EnglishVocab.Identity.Interfaces;

public interface IAuthenService
{
    Task<ServiceResponse<LoginReponse>> LoginAsync(LoginRequest request, string ipAddress);

    Task<ServiceResponse<IdentityResult>> RegisterAsync(RegisterRequest request, string origin);

    Task<RefreshTokenDto> RefreshToken(RefreshTokenDto refreshToken, string ipAddress);
}
