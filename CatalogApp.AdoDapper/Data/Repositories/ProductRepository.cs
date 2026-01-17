using System.Data;
using Dapper; // <--- Це важливо для QueryAsync

namespace CatalogApp.AdoDapper.Data.Repositories
{
    public class ProductRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction? _transaction;

        public ProductRepository(IDbConnection connection, IDbTransaction? transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        // Метод для читання всіх
        public async Task<IEnumerable<dynamic>> GetAllAsync()
        {
            return await _connection.QueryAsync("SELECT * FROM \"Products\"", transaction: _transaction);
        }

        // Метод для читання одного
        public async Task<dynamic?> GetByIdAsync(int id)
        {
            return await _connection.QueryFirstOrDefaultAsync(
                "SELECT * FROM \"Products\" WHERE \"Id\" = @Id", 
                new { Id = id }, 
                transaction: _transaction);
        }
    }
}