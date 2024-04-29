using EnglishVocab.Application.Interfaces;
using EnglishVocab.Application.Producers;
using Microsoft.Extensions.DependencyInjection;

namespace EnglishVocab.Application
{
    public static class ApplicationFactory
    {
        public static void InjectServices(this IServiceCollection services)
        {
            services.AddTransient<IRemindLearnConfigProducers, RemindLearnConfigProducers>();
        }

        public static void InjectMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(ApplicationFactory).Assembly));
        }
    }
}
