using System;
using DSHelper.Data.Sql;

namespace DSHelper.Sample.Data
{
    public class OrderFilter : IOrderFilter
    {
        private static readonly OrderFilter EmptyInstance = new OrderFilter();
        public static OrderFilter Empty { get { return EmptyInstance; }}

        public int? OrderId { get; set; }
        public DateTime? MinCreatedAt { get; set; }
        public DateTime? MaxCreatedAt { get; set; }
        public int? Status { get; set; }

        public string BuildWhereClause(string selectCommandText)
        {
            ConditionsBuilder conditions = new ConditionsBuilder();

            if (OrderId.HasValue)
            {
                conditions.Add(string.Format("OrderId = {0}", OrderId.Value));
            }
            if (MinCreatedAt.HasValue)
            {
                conditions.Add(string.Format("CreatedAt >= {0}", MinCreatedAt.Value));
            }
            if (MaxCreatedAt.HasValue)
            {
                conditions.Add(string.Format("CreatedAt <= {0}", MaxCreatedAt.Value));
            }
            if (Status.HasValue)
            {
                conditions.Add(string.Format("Status = {0}", Status.Value));
            }

            return string.Format("{0} {1}", selectCommandText, conditions.Build());
        }
    }
}
