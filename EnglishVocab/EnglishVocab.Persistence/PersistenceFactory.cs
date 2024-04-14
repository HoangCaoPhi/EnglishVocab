using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnglishVocab.Persistence
{
    public static class PersistenceFactory
    {
        public static void InjectServices(this IServiceCollection services)
        {
            
        }

        public static void InjectDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var appConnection = configuration.GetConnectionString("AppDbConnectionString");
            if (appConnection != null)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseMySQL(appConnection));
            }
            else
            {
                throw new Exception("AppDbConnectionString config missing!");
            }
        }
    }
}
