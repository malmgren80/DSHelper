using System;
using System.Data;

namespace DSHelper.Data.Extensions
{
    public static class DataExtensions
    {
        public static bool IsDirty(this DataRow row)
        {
            if (row == null) 
                throw new ArgumentNullException("row");

            switch(row.RowState)
            {
                case DataRowState.Added:
                case DataRowState.Deleted:
                case DataRowState.Modified:
                    return true;
            }
            return false;
        }
    }
}
