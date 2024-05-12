using EnglishVocab.Domain.Options;
using EnglishVocab.Identity.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EnglishVocab.Identity.Services;
public class JwtTokenService(IOptions<JWTOptions> jwtOptions) : IJwtTokenService
{
    private readonly JWTOptions _jwtOptions = jwtOptions.Value;

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        SigningCredentials signingCredentials;
        if (File.Exists(_jwtOptions.PrivatekeyPath))
        {
            RsaSecurityKey rsaSecurityKey = GenerateRsaKey();
            signingCredentials = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256);
            Console.WriteLine(signingCredentials.ToString());
        }
        else
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTOptions.SecretKey));
            signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        }

        var jwtSecurityToken = GenerateJwtSecurityToken(claims, signingCredentials);
        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    private RsaSecurityKey GenerateRsaKey()
    {
        var rsaKey = RSA.Create();
        string xmlKey = File.ReadAllText(_jwtOptions.PrivatekeyPath);
        rsaKey.FromXmlString(xmlKey);
        var rsaSecurityKey = new RsaSecurityKey(rsaKey);
        return rsaSecurityKey;
    }

    private JwtSecurityToken GenerateJwtSecurityToken(IEnumerable<Claim> claims, SigningCredentials signingCredentials)
    {
        return new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.DurationInMinutes),
            signingCredentials: signingCredentials);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        string refreshToken = "";

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            refreshToken = Convert.ToBase64String(randomNumber);
        }

        return refreshToken;
    }

 

    public ClaimsPrincipal GetPricipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTOptions.SecretKey)),
            ValidateLifetime = false,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}
