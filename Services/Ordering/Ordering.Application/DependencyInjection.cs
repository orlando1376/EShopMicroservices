using BuildingBlocks.Behaviors;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System.Reflection;

namespace Ordering.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(config =>
            {
                // Registra MediatR
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                // Registra el comportamiento de validaciones y registro, Ejecuta los métodos que heredan de AbstractValidator
                // en las clases de CreateOrderCommand, DeleteOrderCommand y UpdateOrderCommand
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));

                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            // Habilita la gestión de funionalidades o características
            // Requiere el nuget Microsoft.FeatureManagement.AspNetCore
            services.AddFeatureManagement();

            // Registrar MassTransit con RabbitMQ. Subscriber
            services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
