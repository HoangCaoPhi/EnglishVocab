using System.Security.Claims;

namespace EnglishVocab.Identity.Interfaces;
public interface IJwtTokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);

    string GenerateRefreshToken();

    ClaimsPrincipal GetPricipalFromExpiredToken(string token);  
}
