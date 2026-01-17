using System.Data;
using CatalogApp.AdoDapper.Data.Interfaces;
using CatalogApp.AdoDapper.Data.Repositories;
using Npgsql;

namespace CatalogApp.AdoDapper.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection? _connection;
        private IDbTransaction? _transaction;
        private readonly string _connectionString;

        // Поля можуть бути null до першого звернення
        private OrderRepository? _orders;
        private ProductRepository? _products;

        public UnitOfWork(IConfiguration configuration)
        {
            // Використовуємо ! (null-forgiving operator), бо ми впевнені, що рядок є
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // Lazy Loading: створюємо тільки коли попросять
        public OrderRepository Orders => _orders ??= new OrderRepository(Connection, _transaction);
        public ProductRepository Products => _products ??= new ProductRepository(Connection, _transaction);

        public IDbConnection Connection
        {
            get
            {
                if (_connection == null) _connection = new NpgsqlConnection(_connectionString);
                if (_connection.State != ConnectionState.Open) _connection.Open();
                return _connection;
            }
        }

        public IDbTransaction? Transaction => _transaction;

        public void BeginTransaction()
        {
            _transaction = Connection.BeginTransaction();
            _orders = null; 
            _products = null;
        }

        public void Commit()
        {
            try { _transaction?.Commit(); }
            catch { _transaction?.Rollback(); throw; }
            finally { _transaction?.Dispose(); _transaction = null; }
        }

        public void Rollback()
        {
            try { _transaction?.Rollback(); }
            finally { _transaction?.Dispose(); _transaction = null; }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}