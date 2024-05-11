using EnglishVocab.Domain.Options;
using EnglishVocab.Identity.Contexts;
using EnglishVocab.Identity.Interfaces;
using EnglishVocab.Identity.Models;
using EnglishVocab.Identity.Services;
using EnglishVocab.Shared.Wrappers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace EnglishVocab.Identity;

public static class ServiceExtensions
{
    public static void AddMySqlIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var appConnection = configuration.GetConnectionString("AppDbConnectionString");
        services.AddDbContext<IdentityContext>(options =>
                   options.UseMySQL(appConnection));

        services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

        services.Configure<JWTOptions>(configuration.GetSection("JWTSettings"));

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;
        });

        services.AddJwtAuthentication(configuration);
    }

    public static void AddIndentityService(this IServiceCollection services)
    {
        services.AddTransient<IAuthenService, AuthenService>();
    }

    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {        
        var jwtOptions = configuration.GetSection(JWTOptions.Name).Get<JWTOptions>();
        RsaSecurityKey rsaSecurityKey = null;
        TokenValidationParameters tokenValidationParameters = null;

        if (File.Exists(jwtOptions.PrivatekeyPath))
        {
            var rsaKey = RSA.Create();
            string xmlKey = File.ReadAllText(jwtOptions.PrivatekeyPath);
            rsaKey.FromXmlString(xmlKey);
            rsaSecurityKey = new RsaSecurityKey(rsaKey);
            tokenValidationParameters = GetTokenValidationParameters(jwtOptions, rsaSecurityKey);
        }
        else
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
            tokenValidationParameters = GetTokenValidationParameters(jwtOptions, symmetricSecurityKey);
        }

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.RequireHttpsMetadata = false;
            o.SaveToken = true;
            o.TokenValidationParameters = tokenValidationParameters;
            o.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = c =>
                {
                    c.Fail("Unauthorized");
                    c.Response.StatusCode = 401;
                    c.Response.ContentType = "application/json";
                    return c.Response.WriteAsync(c.Exception.ToString());
                },
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                    return context.Response.WriteAsync(result);
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";
                    var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
                    return context.Response.WriteAsync(result);
                },
            };
        });
    }

    private static TokenValidationParameters GetTokenValidationParameters(JWTOptions jwtOptions, 
        SecurityKey securityKey)
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = securityKey
        };
    }


}