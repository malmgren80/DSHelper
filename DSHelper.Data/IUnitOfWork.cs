using System;
using System.Data.SqlClient;

namespace DSHelper.Data
{
    public interface IUnitOfWork : IDisposable
    {
        SqlConnection Connection { get; }
        
        void Commit();
        void Rollback();
    }
}
