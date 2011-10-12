using System.Collections.Generic;
using DSHelper.Data;

namespace DSHelper.Sample.Data
{
    public interface IOrderRepository : IRepository<DataSetOrder.OrderRow>
    {
        DataSetOrder.OrderDataTable Orders { get; }
        DataSetOrder.OrderLineDataTable OrderLines { get; }

        IEnumerable<DataSetOrder.OrderRow> Find(IOrderFilter filter);
        IEnumerable<DataSetOrder.OrderLineRow> GetOrderLines(int orderId);

        void Save(DataSetOrder.OrderLineRow orderLine);
        void Delete(DataSetOrder.OrderLineRow orderLine);
    }
}
