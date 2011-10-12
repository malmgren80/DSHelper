using DSHelper.Sample.Data;

namespace DSHelper.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var orderRepository = new OrderRepository();
            var salesOrderSample = new SalesOrderSample(orderRepository);
            salesOrderSample.Run();
        }
    }
}
