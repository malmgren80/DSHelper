using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DSHelper.Data;
using DSHelper.Data.Extensions;
using DSHelper.Data.Sql;

namespace DSHelper.Sample.Data
{
    public class OrderRepository : RepositoryBase, IOrderRepository
    {
        private const string SelectOrderCommandText = "SELECT OrderId, CustomerId, Status, CreatedAt FROM [ORDER]";
        private const string SelectOrderLineCommandText = "SELECT OrderLineId, OrderId, Article FROM [ORDERLINE]";

        private DataSetOrder _dsOrder;

        #region public Properties

        public DataSetOrder.OrderDataTable Orders
        {
            get
            {
                if (_dsOrder == null)
                    _dsOrder = new DataSetOrder();

                return _dsOrder.Order;
            }
        }

        public DataSetOrder.OrderLineDataTable OrderLines
        {
            get
            {
                if (_dsOrder == null)
                    _dsOrder = new DataSetOrder();

                return _dsOrder.OrderLine;
            }
        }

        #endregion

        #region Order methods

        public DataSetOrder.OrderRow Get(int id)
        {
            IOrderFilter filter = new OrderFilter { OrderId = id, };
            Find(filter);
            return Orders.FirstOrDefault();
        }

        public IEnumerable<DataSetOrder.OrderRow> Find(IOrderFilter filter)
        {
            using (var adapter = new SqlDataAdapter(SelectOrderCommandText, Connection))
            {
                adapter.SelectCommand.CommandText = filter.BuildWhereClause(SelectOrderCommandText);
                Orders.Clear();
                adapter.Fill(Orders);
            }

            return Orders;
        }

        public IEnumerable<DataSetOrder.OrderRow> List()
        {
            return Find(OrderFilter.Empty);
        }

        public void Save(DataSetOrder.OrderRow order)
        {
            Orders.AddOrderRow(order);
        }

        public void Delete(DataSetOrder.OrderRow order)
        {
            order.Delete();
        }

        #endregion

        #region OrderLine methods

        public IEnumerable<DataSetOrder.OrderLineRow> GetOrderLines(int orderId)
        {
            var builder = new ConditionsBuilder();
            builder.Add(string.Format("OrderId = {0}", orderId));

            using (var adapter = new SqlDataAdapter(SelectOrderLineCommandText, Connection))
            {
                adapter.SelectCommand.CommandText = string.Format("{0} {1}", SelectOrderLineCommandText, builder.Build());
                OrderLines.Clear();
                adapter.Fill(OrderLines);
            }

            return OrderLines;
        }

        public void Save(DataSetOrder.OrderLineRow orderLine)
        {
            OrderLines.AddOrderLineRow(orderLine);
        }

        public void Delete(DataSetOrder.OrderLineRow orderLine)
        {
            orderLine.Delete();
        }

        #endregion

        #region Update

        public void Update()
        {
            if (_dsOrder != null && Orders.Count(r => r.IsDirty()) > 0)
            {
                using (var adapter = new SqlDataAdapter(SelectOrderCommandText, UnitOfWorkFactory.Current.Connection))
                using (var builder = new SqlCommandBuilder(adapter))
                {
                    adapter.Update(Orders);
                }
            }

            if (_dsOrder != null && OrderLines.Count(r => r.IsDirty()) > 0)
            {
                using (var adapter = new SqlDataAdapter(SelectOrderLineCommandText, UnitOfWorkFactory.Current.Connection))
                using (var builder = new SqlCommandBuilder(adapter))
                {
                    adapter.Update(OrderLines);
                }
            }
        }

        #endregion
    }
}
