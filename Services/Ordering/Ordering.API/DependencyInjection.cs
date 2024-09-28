using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCarter();

            // Adicionar manejo de excepciones personalizadas
            services.AddExceptionHandler<CustomExceptionHandler>();

            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("Database")!); // Requiere el nueget AspNetCore.HealthChecks.SqlServer

            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.MapCarter();

            // Middleware para el manejo de excepciones
            app.UseExceptionHandler(options => { });

            app.UseHealthChecks("/health",
                // Convertir la salida en un formato JSON mas legible
                new HealthCheckOptions // Requiere el nuget AspNetCore.HealthChecks.UI.Client
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

            return app;
        }
    }
}
