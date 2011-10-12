using System;
using DSHelper.Sample.Data;

namespace DSHelper.Sample
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly Random _random;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;

            _random = new Random();
        }

        public DataSetOrder.OrderRow CreateOrder()
        {
            var order = _orderRepository.Orders.NewOrderRow();

            int orderId = _random.Next();

            order.OrderId = orderId;
            order.CustomerId = 3;
            order.CreatedAt = DateTime.Now;
            order.Status = 5;
            _orderRepository.Save(order);

            CreateAndPersistOrderLine(orderId);
            CreateAndPersistOrderLine(orderId);

            _orderRepository.Update();

            return order;
        }

        private void CreateAndPersistOrderLine(int orderId)
        {
            var line = _orderRepository.OrderLines.NewOrderLineRow();
            line.OrderLineId = _random.Next();
            line.OrderId = orderId;
            line.Article = "Fancy Pants";
            _orderRepository.Save(line);
        }
    }
}
