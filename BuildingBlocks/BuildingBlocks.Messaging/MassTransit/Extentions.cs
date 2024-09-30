using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit
{
    public static class Extentions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
        {
            services.AddMassTransit(config =>
            {
                // Establecer el convenio de nomenclatura de comunicación KebaCase
                config.SetKebabCaseEndpointNameFormatter();

                // Si biene un assemby se registra como consumer
                if (assembly != null)
                    config.AddConsumers(assembly);

                config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                    {
                        host.Username(configuration["MessageBroker:UserName"]);
                        host.Password(configuration["MessageBroker:Password"]);
                    });

                    // Configurar endpoint
                    configurator.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
