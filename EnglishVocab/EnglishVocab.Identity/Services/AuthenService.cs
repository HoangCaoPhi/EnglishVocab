using EnglishVocab.Domain.Enums;
using EnglishVocab.Domain.Options;
using EnglishVocab.Identity.Contexts;
using EnglishVocab.Identity.Dtos;
using EnglishVocab.Identity.Dtos.Requests.Authen;
using EnglishVocab.Identity.Dtos.Responses.Authen;
using EnglishVocab.Identity.Interfaces;
using EnglishVocab.Identity.Models;
using EnglishVocab.Identity.Utils;
using EnglishVocab.Shared.Exceptions;
using EnglishVocab.Shared.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EnglishVocab.Identity.Services;
public class AuthenService(UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    RoleManager<IdentityRole> roleManager,
    IdentityContext context,
    IOptions<JWTOptions> jwtOption) : IAuthenService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly JWTOptions _jwtOptions = jwtOption.Value;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IdentityContext _context = context; 

    public async Task<Response<LoginReponse>> LoginAsync(LoginRequest request, string ipAddress)
    {
        var user = await _userManager.FindByEmailAsync(request.Email) ?? 
                  throw new ApiException($"No Accounts Registered with {request.Email}.");

        var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            throw new ApiException($"Invalid Credentials for '{request.Email}'.");
        }
 
        JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

        LoginReponse response = new ()
        {
            Id = user.Id,
            JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Email = user.Email,
            UserName = user.UserName
        };

        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        response.Roles = rolesList.ToList();

        response.IsVerified = user.EmailConfirmed;

        var refreshToken = await GenerateRefreshToken(ipAddress, user);
        response.RefreshToken = refreshToken.Token;

        return new Response<LoginReponse>(response, $"Authenticated {user.UserName}");
    }



    private async Task<string> GetPermissionOfRole(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        var roleClaimsForRole = _context.RoleClaims.Where(rc => rc.RoleId == role.Id).ToList();
        List<RolePermission> rolePermissions = new();

        foreach (var item in roleClaimsForRole)
        {
            rolePermissions.Add(new RolePermission
            {
                Resource = item.ClaimType,
                Action = item.ClaimValue.Split("#")
            });
        }

        return JsonConvert.SerializeObject(
                new
                {
                    Role = roleName,
                    Permissions = rolePermissions
                });
    }

    private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim("roles", await GetPermissionOfRole(roles[i].ToString())));
        }

        string ipAddress = IdentityUtils.GetIpAddress();

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("fullname", user.FirstName + " " + user.LastName),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress)
        }
        .Union(userClaims)
        .Union(roleClaims);

        SigningCredentials signingCredentials;
        if (File.Exists(_jwtOptions.PrivatekeyPath))
        {
            RsaSecurityKey rsaSecurityKey = GenerateRsaKey();
            signingCredentials = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256);
            Console.WriteLine(signingCredentials.ToString());
        }
        else
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        }

        return GenerateJwtSecurityToken(claims, signingCredentials);
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


    public async Task<Response<IdentityResult>> RegisterAsync(RegisterRequest request, string origin)
    {
        var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
        if (userWithSameUserName != null)
        {
            throw new ApiException($"Username '{request.UserName}' is already taken.");
        }
        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName
        };
        var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameEmail == null)
        {
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                return new Response<IdentityResult>(result);
            }
            else
            {
                throw new ApiException($"{result.Errors}");
            }
        }
        else
        {
            throw new ApiException($"Email {request.Email} is already registered.");
        }
    }

    private async Task<RefreshToken> GenerateRefreshToken(string ipAddress, ApplicationUser user)
    {
        var refreshToken = new RefreshToken
        {
            Token = RandomTokenString(),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress            
        };
        user.RefreshTokens = new List<RefreshToken> { refreshToken };
        await _userManager.UpdateAsync(user);
        return refreshToken;
    }

    private string RandomTokenString()
    {
        using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        var randomBytes = new byte[40];
        rngCryptoServiceProvider.GetBytes(randomBytes);
        return BitConverter.ToString(randomBytes).Replace("-", "");
    }
 
    public async Task<RefreshTokenDto> RefreshToken(RefreshTokenDto refreshToken, string ipAddress)
    {
        var principal = GetPrincipalFromExpiredToken(refreshToken.AccessToken) ?? throw new SecurityTokenException("Invalid token");

        var uid = principal.FindFirst("uid")?.Value;
        var user = await _userManager
            .Users.Include(x => x.RefreshTokens
            .Where(t =>
                t.Token == refreshToken.RefreshToken
                && t.Expires > DateTime.UtcNow
                && t.CreatedByIp == ipAddress
            ))
        .FirstAsync(x => x.Id == uid);
        if (user.RefreshTokens.Count > 0)
        {
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            var newRefreshToken = await GenerateRefreshToken(ipAddress, user);
            return new RefreshTokenDto()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = newRefreshToken.Token
            };
        }
        throw new SecurityTokenException("Invalid token");
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
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
