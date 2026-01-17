using Dapper;
using Npgsql;

namespace CatalogApp.AdoDapper.Data
{
    public class DbInitializer
    {
        private readonly string _connectionString;

        public DbInitializer(IConfiguration configuration)
        {
            // Додали '!', щоб прибрати попередження CS8601/CS8618
            // Це означає: "Ми впевнені, що рядок підключення є в appsettings.json"
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public void Initialize()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            // 1. Таблиця Товарів (Довідник)
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS ""Products"" (
                    ""Id"" SERIAL PRIMARY KEY,
                    ""Name"" TEXT NOT NULL,
                    ""Price"" DECIMAL(18,2) NOT NULL
                );");

            // 2. Таблиця Замовлень
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS ""Orders"" (
                    ""Id"" SERIAL PRIMARY KEY,
                    ""CustomerName"" TEXT NOT NULL,
                    ""OrderDate"" TIMESTAMP NOT NULL,
                    ""TotalAmount"" DECIMAL(18,2) NOT NULL,
                    ""Status"" TEXT NOT NULL
                );");

            // 3. Таблиця Позицій замовлення
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS ""OrderItems"" (
                    ""Id"" SERIAL PRIMARY KEY,
                    ""OrderId"" INTEGER NOT NULL REFERENCES ""Orders""(""Id"") ON DELETE CASCADE,
                    ""ProductId"" INTEGER NOT NULL,
                    ""ProductName"" TEXT NOT NULL,
                    ""UnitPrice"" DECIMAL(18,2) NOT NULL,
                    ""Quantity"" INTEGER NOT NULL
                );");

            // Заповнимо тестовими товарами, якщо їх немає
            var count = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM \"Products\"");
            if (count == 0)
            {
                connection.Execute(@"
                    INSERT INTO ""Products"" (""Name"", ""Price"") VALUES 
                    ('Ноутбук Gaming X', 45000),
                    ('Мишка бездротова', 1200),
                    ('Клавіатура механічна', 3500);
                ");
            }
        }
    }
}