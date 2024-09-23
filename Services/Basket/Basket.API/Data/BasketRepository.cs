namespace Basket.API.Data
{
    public class BasketRepository(IDocumentSession session) : IBasketRepository
    {
        /// <summary>
        /// Obtiene un carrito de compra
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="BasketNotFoundException"></exception>
        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            // Obtener el carrito de compra por usuario
            var basket = await session.LoadAsync<ShoppingCart>(userName, cancellationToken);

            // si no existe el carrito de compra se lanza una excepción
            return basket is null ? throw new BasketNotFoundException(userName) : basket;
        }

        /// <summary>
        /// Almacena un carrito de compra
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            // Crea o actualiza el carrito de compra a travez de Marten
            session.Store(basket);
            await session.SaveChangesAsync(cancellationToken);

            return basket;
        }

        /// <summary>
        /// Borra un carrito de compra
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            // Borra el carrito de compra po usuario
            session.Delete<ShoppingCart>(userName);
            await session.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
