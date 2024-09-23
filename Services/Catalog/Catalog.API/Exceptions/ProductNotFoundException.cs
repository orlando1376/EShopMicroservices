using BuildingBlocks.Exceptions;

namespace Catalog.API.Exceptions
{
    // Hereda de la clase de exception personalizada
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid id) : base("Product", id)
        {            
        }
    }
}
