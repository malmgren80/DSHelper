using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;

namespace DSHelper.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private SqlConnection _connection;
        private readonly TransactionScope _transaction;
        private bool _disposed;

        public UnitOfWork()
        {
            _transaction = new TransactionScope();
        }

        public SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
                    _connection = new SqlConnection(connectionString);
                }

                return _connection;
            }
        }

        public void Commit()
        {
            if (_disposed) throw new InvalidOperationException("UnitOfWork is disposed!");
            
            _transaction.Complete();
            _transaction.Dispose();
        }

        public void Rollback()
        {
            _transaction.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                if (_transaction != null)
                    _transaction.Dispose();
                
                if (_connection != null)
                    _connection.Dispose();
            }

            _disposed = true;
        }
    }
}
