using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHelper.Data;
using DSHelper.Sample.Data;

namespace DSHelper.Sample
{
    public class SalesOrderSample
    {
        private readonly IOrderRepository _orderRepository;
        private readonly Random _random;

        public SalesOrderSample(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;

            _random = new Random();
        }

        public void Run()
        {
            IUnitOfWork unitOfWork = UnitOfWorkFactory.Start();
            
            var order = CreateOrder();
            
            unitOfWork.Commit();
            Console.WriteLine(string.Format("Order with OrderId: {0} saved.", order.SalesOrderID));

            Console.WriteLine("Press a key to continue");
            Console.ReadKey();

            unitOfWork = UnitOfWorkFactory.Start();

            order = _orderRepository.Get(order.SalesOrderID);

            if (order == null)
                Console.WriteLine("Order is null");
            else
                Console.WriteLine("Order was found!");

            unitOfWork.Commit();

            Console.WriteLine("Press a key to continue");
            Console.ReadKey();
        }

        private DataSetAdventureWorks.SalesOrderHeaderRow CreateOrder()
        {
            var order = _orderRepository.Orders.NewSalesOrderHeaderRow();

            int orderId = _random.Next();

            order.SalesOrderID = orderId;
            order.RevisionNumber = 1;
            order.OrderDate = DateTime.Now;
            order.DueDate = DateTime.Now.AddYears(5);
            order.Status = 5;
            order.OnlineOrderFlag = true;
            order.SalesOrderNumber = "FANCYPANT01";
            order.CustomerID = 1;
            order.ShipMethod = "boat";
            order.SubTotal = 10;
            order.TaxAmt = 3;
            order.Freight = 5;
            order.TotalDue = 0;
            order.rowguid = Guid.NewGuid();
            order.ModifiedDate = DateTime.Now;
            _orderRepository.Save(order);

            CreateAndPersistOrderLine(orderId);
            CreateAndPersistOrderLine(orderId);

            _orderRepository.Update();

            return order;
        }

        private void CreateAndPersistOrderLine(int orderId)
        {
            var line = _orderRepository.OrderLines.NewSalesOrderDetailRow();
            line.SalesOrderDetailID = _random.Next();
            line.SalesOrderID = orderId;
            line.OrderQty = 123;
            line.ProductID = 1;
            line.UnitPrice = 10;
            line.UnitPriceDiscount = 1;
            line.LineTotal = 1230;
            line.rowguid = Guid.NewGuid();
            line.ModifiedDate = DateTime.Now;
            _orderRepository.Save(line);
        }
    }
}
