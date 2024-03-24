using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ponto.Eletronico.Core.Interfaces;

namespace Ponto.Eletronico.Infrastructure.Messaging
{
    public static class EventServiceCollection
    {
        public static IServiceCollection AddServiceBus(this IServiceCollection services, IConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            services.AddSingleton<IServiceBusProducer, ServiceBusProducer>();

            return services;
        }
    }
}
