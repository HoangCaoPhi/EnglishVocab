using EnglishVocab.Application.Interfaces;
using EnglishVocab.Application.Producers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EnglishVocab.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationService(this IServiceCollection services)
        {
            services.AddTransient<IRemindLearnConfigProducers, RemindLearnConfigProducers>();
        }

        public static void AddMediatRApp(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}
