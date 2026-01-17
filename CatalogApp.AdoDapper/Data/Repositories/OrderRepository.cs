using System.Data;
using CatalogApp.AdoDapper.Data.Entities;
using Dapper; // Використаємо Dapper для Update/Delete бо це швидше писати
using Npgsql;

namespace CatalogApp.AdoDapper.Data.Repositories
{
    public class OrderRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction? _transaction;

        public OrderRepository(IDbConnection connection, IDbTransaction? transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        // --- CREATE (ADO.NET) ---
        public int Create(Order order)
        {
            using var command = _connection.CreateCommand();
            command.Transaction = _transaction;
            command.CommandText = @"
                INSERT INTO ""Orders"" (""CustomerName"", ""OrderDate"", ""TotalAmount"", ""Status"") 
                VALUES (@name, @date, @total, @status) 
                RETURNING ""Id"";";

            AddParameter(command, "name", (object?)order.CustomerName ?? DBNull.Value);
            AddParameter(command, "date", order.OrderDate);
            AddParameter(command, "total", order.TotalAmount);
            AddParameter(command, "status", (object?)order.Status ?? DBNull.Value);

            var result = command.ExecuteScalar();
            var newId = Convert.ToInt32(result);
            order.Id = newId;

            foreach (var item in order.Items)
            {
                using var itemCmd = _connection.CreateCommand();
                itemCmd.Transaction = _transaction;
                itemCmd.CommandText = @"
                    INSERT INTO ""OrderItems"" (""OrderId"", ""ProductId"", ""ProductName"", ""UnitPrice"", ""Quantity"") 
                    VALUES (@oid, @pid, @pname, @price, @qty);";

                AddParameter(itemCmd, "oid", newId);
                AddParameter(itemCmd, "pid", item.ProductId);
                AddParameter(itemCmd, "pname", (object?)item.ProductName ?? DBNull.Value);
                AddParameter(itemCmd, "price", item.UnitPrice);
                AddParameter(itemCmd, "qty", item.Quantity);

                itemCmd.ExecuteNonQuery();
            }
            return newId;
        }

        // --- UPDATE STATUS (Dapper) ---
        public async Task UpdateStatusAsync(int id, string status)
        {
            await _connection.ExecuteAsync(
                "UPDATE \"Orders\" SET \"Status\" = @Status WHERE \"Id\" = @Id",
                new { Id = id, Status = status },
                transaction: _transaction);
        }

        // --- DELETE (Dapper) ---
        public async Task DeleteAsync(int id)
        {
            // Каскадне видалення OrderItems налаштоване в базі, тому видаляємо тільки Order
            await _connection.ExecuteAsync(
                "DELETE FROM \"Orders\" WHERE \"Id\" = @Id",
                new { Id = id },
                transaction: _transaction);
        }

        // --- GET BY ID (Dapper) ---
        public async Task<Order?> GetByIdAsync(int id)
        {
            // Спочатку саме замовлення
            var order = await _connection.QueryFirstOrDefaultAsync<Order>(
                "SELECT * FROM \"Orders\" WHERE \"Id\" = @Id",
                new { Id = id }, transaction: _transaction);

            if (order != null)
            {
                // Потім його позиції
                var items = await _connection.QueryAsync<OrderItem>(
                    "SELECT * FROM \"OrderItems\" WHERE \"OrderId\" = @OrderId",
                    new { OrderId = id }, transaction: _transaction);
                
                order.Items = items.ToList();
            }
            return order;
        }

        private void AddParameter(IDbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }
    }
}