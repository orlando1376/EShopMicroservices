using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    // NOTA: Requiere el paquete Microsoft.Extensions.Caching.StackExchangeRedis

    public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            // Obtener información del cache
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
            if (!string.IsNullOrEmpty(cachedBasket))
            {
                // Deserializar y devolver como JSON el valor obtenido del cache
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
            }

            // Si no existe la información el cache se obtiene de la base de datos
            var basket = await repository.GetBasket(userName, cancellationToken);

            // Guardar información en el cache
            await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            // Guardar información en la base de datos
            await repository.StoreBasket(basket, cancellationToken);

            // Guardar información en el cache
            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            // Borrar la información en la base de datos
            await repository.DeleteBasket(userName, cancellationToken);

            // Borrar la información en el cache
            await cache.RemoveAsync(userName, cancellationToken);

            return true;
        }
    }
}
