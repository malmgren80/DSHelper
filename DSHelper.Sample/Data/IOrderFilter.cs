using System;

namespace DSHelper.Sample.Data
{
    public interface IOrderFilter
    {
        int? OrderId { get; set; }
        DateTime? MinCreatedAt { get; set; }
        DateTime? MaxCreatedAt { get; set; }
        int? Status { get; set; }

        string BuildWhereClause(string selectCommandText);
    }
}
