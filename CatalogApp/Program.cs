using CatalogApp.Data;
using CatalogApp.Interfaces;
using CatalogApp.Middleware; // <--- Middleware
using CatalogApp.Helpers;    // <--- Mapper & Validation
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. База даних
builder.Services.AddDbContext<CatalogContext>(options =>
    options.UseSqlite("Data Source=catalog.db"));

// 2. Репозиторії
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// 3. AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfiles));

// 4. FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();
builder.Services.AddFluentValidationAutoValidation(); 

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 5. Підключаємо Middleware помилок (найпершим!)
app.UseMiddleware<ExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CatalogContext>();
        await context.Database.EnsureCreatedAsync();
        
        // Seeding (якщо треба)
        if (!context.Products.Any())
        {
            var cat1 = new CatalogApp.Models.Category { Name = "Електроніка" };
            context.Categories.Add(cat1);
            context.Products.Add(new CatalogApp.Models.Product { Name = "Test Product", Price = 100, ProductCategory = cat1 });
            await context.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during migration");
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

app.Run();