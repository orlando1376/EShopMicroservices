namespace Basket.API.Exceptions
{
    // Hereda de la clase de exception personalizada
    public class BasketNotFoundException : NotFoundException
    {
        public BasketNotFoundException(string userName) : base("Basket", userName)
        {
        
        }
    }
}
