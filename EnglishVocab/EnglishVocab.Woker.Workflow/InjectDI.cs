using EnglishVocab.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EnglishVocab.Persistence
{
    public static class InjectDI
    {
        public static void InjectServices(this IServiceCollection services)
        {
            
        }

        public static void InjectDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var appConnection = configuration.GetConnectionString("AppDbConnectionString");
            if (appConnection != null)
            {
                services.AddDbContext<ScheduleContext>(options =>
                            options.UseMySQL(appConnection));
            }
            else
            {
                throw new Exception("AppDbConnectionString config missing!");
            }
        }
    }
}
