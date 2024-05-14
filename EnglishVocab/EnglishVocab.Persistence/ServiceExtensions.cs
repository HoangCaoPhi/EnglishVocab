using EnglishVocab.Application.Interfaces.Repos;
using EnglishVocab.Domain.Interfaces;
using EnglishVocab.Persistence.Contexts;
using EnglishVocab.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnglishVocab.Persistence
{
    public static class ServiceExtensions
    {
        public static void AddPersistenceService(this IServiceCollection services)
        {
            services.AddTransient<IGroupRepo, GroupRepo>();
            services.AddTransient<IWordRepo, WordRepo>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        public static void AddDbAppContext(this IServiceCollection services, IConfiguration configuration)
        {
            var appConnection = configuration.GetConnectionString("AppDbConnectionString");
            if (appConnection != null)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseMySQL(appConnection);
                });
            }
            else
            {
                throw new Exception("AppDbConnectionString config missing!");
            }
        }
    }
}
