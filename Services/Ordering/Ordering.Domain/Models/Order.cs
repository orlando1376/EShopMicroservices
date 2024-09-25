namespace Ordering.Domain.Models
{
    public class Order : Aggregate<OrderId>
    {
        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
        public CustomerId CustomerId { get; private set; } = default!;
        public OrderName OrderName { get; private set; } = default!;
        /// <summary>
        /// Dirección de envío
        /// </summary>
        public Address ShippingAddress { get; private set; } = default!;
        /// <summary>
        /// Dirección de facturación
        /// </summary>
        public Address BillingAddress { get; private set; } = default!;
        public Payment Payment { get; private set; } = default!;
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public decimal TotalPrice
        {
            get => OrderItems.Sum(x => x.Price * x.Quantity);
            private set { }
        }

        /// <summary>
        /// Crea una nueva Orden
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customerId"></param>
        /// <param name="orderName"></param>
        /// <param name="shippingAddress"></param>
        /// <param name="billingAddress"></param>
        /// <param name="payment"></param>
        /// <returns></returns>
        public static Order Create(OrderId id, CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment)
        {
            var order = new Order
            {
                Id = id,
                CustomerId = customerId,
                OrderName = orderName,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                Payment = payment,
                Status = OrderStatus.Pending
            };

            // Adicionar evento
            order.AddDomainEvent(new OrderCreatedEvent(order));

            return order;
        }

        /// <summary>
        /// Actualiza una Orden
        /// </summary>
        /// <param name="orderName"></param>
        /// <param name="shippingAddress"></param>
        /// <param name="billingAddress"></param>
        /// <param name="payment"></param>
        /// <param name="status"></param>
        public void Update(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment, OrderStatus status)
        {
            OrderName = orderName;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            Payment = payment;
            Status = status;

            // Actualizar evento
            AddDomainEvent(new OrderUpdatedEvent(this));
        }

        /// <summary>
        /// Adiciona un producto a la Orden
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        public void Add(ProductId productId, int quantity, decimal price)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var orderItem = new OrderItem(Id, productId, quantity, price);
            _orderItems.Add(orderItem);
        }

        /// <summary>
        /// Borra un produecto de la Orden
        /// </summary>
        /// <param name="productId"></param>
        public void Remove(ProductId productId)
        {
            var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
            if (orderItem is not null)
            {
                _orderItems.Remove(orderItem);
            }
        }
    }
}
