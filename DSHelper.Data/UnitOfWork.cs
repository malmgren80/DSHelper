using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;

namespace DSHelper.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TransactionScope _transaction;
        private bool _disposed;
        private DbConnection _connection;

        public UnitOfWork()
        {
            _transaction = new TransactionScope();
        }

        public DbConnection Connection
        {
            get { return _connection ?? (_connection = CreateConnection()); }
        }

        // TODO: Move to ConnectionManager...
        private static DbConnection CreateConnection()
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings[0];
            var factoryClasses = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
            var connection = factoryClasses.CreateConnection();

            if (connection == null)
            {
                throw new NullReferenceException(
                    "It's wasn't possible to create the connection for connection string.");
            }

            connection.ConnectionString = connectionStringSettings.ConnectionString;
            connection.Open();

            return connection;
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
