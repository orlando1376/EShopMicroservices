namespace Basket.API.Data
{
    public interface IBasketRepository
    {
        /// <summary>
        /// Obtiene un carrito de compra
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Almacena un carrito de compra
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default);

        /// <summary>
        /// Borra un carrito de compra
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default);
    }
}
