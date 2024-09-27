using BuildingBlocks.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ordering.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
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

            return services;
        }
    }
}
