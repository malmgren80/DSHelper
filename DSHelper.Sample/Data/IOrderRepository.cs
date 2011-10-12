using System.Collections.Generic;
using DSHelper.Data;

namespace DSHelper.Sample.Data
{
    public interface IOrderRepository : IRepository<DataSetAdventureWorks.SalesOrderHeaderRow>
    {
        DataSetAdventureWorks.SalesOrderHeaderDataTable Orders { get; }
        DataSetAdventureWorks.SalesOrderDetailDataTable OrderLines { get; }

        IEnumerable<DataSetAdventureWorks.SalesOrderHeaderRow> Find(IOrderFilter filter);
        IEnumerable<DataSetAdventureWorks.SalesOrderDetailRow> GetOrderLines(int orderId);

        void Save(DataSetAdventureWorks.SalesOrderDetailRow orderLine);
        void Delete(DataSetAdventureWorks.SalesOrderDetailRow orderLine);
    }
}
