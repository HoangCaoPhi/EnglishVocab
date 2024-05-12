using EnglishVocab.Domain.Options;
using EnglishVocab.Identity.Contexts;
using EnglishVocab.Identity.Interfaces;
using EnglishVocab.Identity.Models;
using EnglishVocab.Identity.Services;
using EnglishVocab.Shared.Wrappers;
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

        //services.Configure<IdentityOptions>(options =>
        //{
        //    // Password settings.
        //    options.Password.RequireDigit = true;
        //    options.Password.RequireLowercase = true;
        //    options.Password.RequireNonAlphanumeric = true;
        //    options.Password.RequireUppercase = true;
        //    options.Password.RequiredLength = 6;
        //    options.Password.RequiredUniqueChars = 1;

        //    // Lockout settings.
        //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        //    options.Lockout.MaxFailedAccessAttempts = 5;
        //    options.Lockout.AllowedForNewUsers = true;

        //    // User settings.
        //    options.User.AllowedUserNameCharacters =
        //    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        //    options.User.RequireUniqueEmail = false;
        //});

        services.AddJwtAuthentication(configuration);
    }

    public static void AddIndentityService(this IServiceCollection services)
    {
        services.AddTransient<IAuthenService, AuthenService>();
        services.AddTransient<IJwtTokenService, JwtTokenService>();
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
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTOptions.SecretKey));
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
            o.SaveToken = false;
            o.TokenValidationParameters = tokenValidationParameters;
            o.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = context =>
                {
                    var errorResult = JsonConvert.SerializeObject(new ServiceResponse<string>("Unauthorized: " + context.Exception.ToString()));
                    context.Response.WriteAsync(errorResult);
                    context.Fail("Unauthorized");  
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    context.HandleResponse(); 
                    var result = JsonConvert.SerializeObject(new ServiceResponse<string>("You are not Authorized"));
                    context.Response.WriteAsync(result);
                    return Task.CompletedTask;
                },
                OnForbidden = context =>
                {
                    var result = JsonConvert.SerializeObject(new ServiceResponse<string>("You are not authorized to access this resource"));
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