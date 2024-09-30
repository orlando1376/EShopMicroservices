using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers.Domain
{
    public class OrderCreatedEventHandler(IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> logger) : INotificationHandler<OrderCreatedEvent>
    {
        public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

            // Validar si la característica o servicio de integración ya está disponible,
            // esto para impedir que cuando se creen los registros de simbra de datos no se active el evento de integración
            // IFeatureManager requiere el nuget Microsoft.FeatureManagement.AspNetCore
            if (await featureManager.IsEnabledAsync("OrderFullfilment")) // La varible OrderFullfilment está el appsettings
            {
                // Convertir el evento de dominio al DTO de orden
                var orderCreatedIntegrationEvent = domainEvent.order.ToOrderDto();

                // Publicar evento de integración en RabbitMQ
                await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
            }
        }
    }
}
