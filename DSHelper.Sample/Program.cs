using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using DSHelper.Data;
using DSHelper.Sample.Data;
using Microsoft.Practices.ServiceLocation;

namespace DSHelper.Sample
{
    class Program
    {
        private static IWindsorContainer _container;

        static void Main(string[] args)
        {
            // Init ioc-container
            _container = new WindsorContainer();

            // Register services in container
            _container.Register(Component.For<IOrderRepository>().ImplementedBy<OrderRepository>().LifeStyle.Singleton);
            _container.Register(Component.For<IOrderService>().ImplementedBy<OrderService>().LifeStyle.Singleton);

            // Init CommonServiceLocator
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(_container));

            Program p = new Program();
            p.Run();
            
            Console.ReadKey();
        }

        public void Run()
        {
            IUnitOfWork unitOfWork = UnitOfWorkFactory.Start();
            
            IOrderService orderService = _container.Resolve<IOrderService>();
            var order = orderService.CreateOrder();

            unitOfWork.Commit();
            Console.WriteLine(string.Format("Order with OrderId: {0} saved.", order.OrderId));

            Console.WriteLine("Press a key to continue");
            Console.ReadKey();

            unitOfWork = UnitOfWorkFactory.Start();

            IOrderRepository orderRepository = _container.Resolve<IOrderRepository>();
            order = orderRepository.Get(order.OrderId);

            if (order == null)
                Console.WriteLine("Order is null");
            else
                Console.WriteLine("Order was found!");

            unitOfWork.Commit();

            Console.WriteLine("Press a key to continue");
            Console.ReadKey();
        }
    }
}
