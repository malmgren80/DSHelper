using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace DSHelper.Data
{
    public interface IUnitOfWork : IDisposable
    {
        DbConnection Connection { get; }
        
        void Commit();
        void Rollback();
    }
}
