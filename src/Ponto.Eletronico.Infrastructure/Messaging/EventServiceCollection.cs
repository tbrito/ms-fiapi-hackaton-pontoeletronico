using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ponto.Eletronico.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            services.Configure<ServiceBusSettings>(config.GetSection("ServiceBusSettings"));

            services.AddSingleton<IServiceBusProducer, ServiceBusProducer>();

            return services;
        }
    }
}
