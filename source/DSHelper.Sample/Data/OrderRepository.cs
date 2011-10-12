using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using DSHelper.Data;
using DSHelper.Data.Extensions;
using DSHelper.Data.Sql;

namespace DSHelper.Sample.Data
{
    public class OrderRepository : RepositoryBase, IOrderRepository
    {
        private const string SelectOrderCommandText = "SELECT * FROM SalesOrderHeader";
        private const string SelectOrderLineCommandText = "SELECT * FROM SalesOrderDetail";

        private DataSetAdventureWorks _dsOrder;

        #region public Properties

        public DataSetAdventureWorks.SalesOrderHeaderDataTable Orders
        {
            get
            {
                if (_dsOrder == null)
                    _dsOrder = new DataSetAdventureWorks();

                return _dsOrder.SalesOrderHeader;
            }
        }

        public DataSetAdventureWorks.SalesOrderDetailDataTable OrderLines
        {
            get
            {
                if (_dsOrder == null)
                    _dsOrder = new DataSetAdventureWorks();

                return _dsOrder.SalesOrderDetail;
            }
        }

        #endregion

        #region Order methods

        public DataSetAdventureWorks.SalesOrderHeaderRow Get(int id)
        {
            IOrderFilter filter = new OrderFilter { OrderId = id, };
            Find(filter);
            return Orders.FirstOrDefault();
        }

        public IEnumerable<DataSetAdventureWorks.SalesOrderHeaderRow> Find(IOrderFilter filter)
        {
            using (var adapter = new SqlDataAdapter(SelectOrderCommandText, Connection as SqlConnection))
            {
                adapter.SelectCommand.CommandText = filter.BuildWhereClause(SelectOrderCommandText);
                Orders.Clear();
                adapter.Fill(Orders);
            }

            return Orders;
        }

        public IEnumerable<DataSetAdventureWorks.SalesOrderHeaderRow> List()
        {
            return Find(OrderFilter.Empty);
        }

        public void Save(DataSetAdventureWorks.SalesOrderHeaderRow order)
        {
            Orders.AddSalesOrderHeaderRow(order);
        }

        public void Delete(DataSetAdventureWorks.SalesOrderHeaderRow order)
        {
            order.Delete();
        }

        #endregion

        #region OrderLine methods

        public IEnumerable<DataSetAdventureWorks.SalesOrderDetailRow> GetOrderLines(int orderId)
        {
            var builder = new ConditionsBuilder();
            builder.Add(string.Format("OrderId = {0}", orderId));
            using (var adapter = new SqlDataAdapter(SelectOrderLineCommandText, Connection as SqlConnection))
            {
                adapter.SelectCommand.CommandText = string.Format("{0} {1}", SelectOrderLineCommandText, builder.Build());
                OrderLines.Clear();
                adapter.Fill(OrderLines);
            }

            return OrderLines;
        }

        public void Save(DataSetAdventureWorks.SalesOrderDetailRow orderLine)
        {
            OrderLines.AddSalesOrderDetailRow(orderLine);
        }

        public void Delete(DataSetAdventureWorks.SalesOrderDetailRow orderLine)
        {
            orderLine.Delete();
        }

        #endregion

        #region Update

        public void Update()
        {
            if (_dsOrder != null && Orders.Count(r => r.IsDirty()) > 0)
            {
                using (var adapter = new SqlDataAdapter(SelectOrderCommandText, UnitOfWorkFactory.Current.Connection as SqlConnection))
                using (var builder = new SqlCommandBuilder(adapter))
                {
                    adapter.Update(Orders);
                }
            }

            if (_dsOrder != null && OrderLines.Count(r => r.IsDirty()) > 0)
            {
                using (var adapter = new SqlDataAdapter(SelectOrderLineCommandText, UnitOfWorkFactory.Current.Connection as SqlConnection))
                using (var builder = new SqlCommandBuilder(adapter))
                {
                    adapter.Update(OrderLines);
                }
            }
        }

        #endregion
    }
}
