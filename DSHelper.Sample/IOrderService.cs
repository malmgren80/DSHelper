using DSHelper.Sample.Data;

namespace DSHelper.Sample
{
    public interface IOrderService
    {
        DataSetOrder.OrderRow CreateOrder();
    }
}
