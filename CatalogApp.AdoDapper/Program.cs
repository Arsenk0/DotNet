using CatalogApp.AdoDapper.Data;
using CatalogApp.AdoDapper.Data.Interfaces;
using CatalogApp.AdoDapper.Data.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using CatalogApp.AdoDapper.Validators;

var builder = WebApplication.CreateBuilder(args);

// 1. Налаштування Kestrel (щоб точно слухав порт 5200)
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5200);
});

// 2. Реєстрація сервісів
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<DbInitializer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // <--- Це обов'язково для Swagger
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderDtoValidator>();

var app = builder.Build();

// 3. Ініціалізація БД
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        var initializer = services.GetRequiredService<DbInitializer>();
        initializer.Initialize();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error initializing database: {ex.Message}");
    }
}

// 4. Вмикаємо Swagger (БЕЗ умов if development)
app.UseSwagger();
app.UseSwaggerUI(); // Це створює сторінку /swagger/index.html

app.UseAuthorization();
app.MapControllers();

app.Run();