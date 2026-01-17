using CatalogApp.Mongo.Entities;
using MediatR;
using MongoDB.Driver;

namespace CatalogApp.Mongo.Features.Reviews.Queries.GetProductStats
{
    public class GetProductStatsHandler : IRequestHandler<GetProductStatsQuery, ProductStatsDto>
    {
        private readonly IMongoCollection<Review> _reviews;

        public GetProductStatsHandler(IMongoDatabase database)
        {
            _reviews = database.GetCollection<Review>("reviews");
        }

        public async Task<ProductStatsDto> Handle(GetProductStatsQuery request, CancellationToken cancellationToken)
        {
            // --- АГРЕГАЦІЯ ---
            var stats = await _reviews.Aggregate()
                // Етап 1: Фільтруємо (беремо тільки цей товар)
                .Match(r => r.ProductId == request.ProductId)
                // Етап 2: Групуємо і рахуємо
                .Group(r => r.ProductId, g => new 
                {
                    ProductId = g.Key,
                    AverageRating = g.Average(r => r.Rating),
                    Count = g.Count()
                })
                .FirstOrDefaultAsync(cancellationToken);

            // Якщо відгуків немає, повертаємо нулі
            if (stats == null)
            {
                return new ProductStatsDto 
                { 
                    ProductId = request.ProductId, 
                    AverageRating = 0, 
                    ReviewsCount = 0 
                };
            }

            // Мапимо результат у DTO
            return new ProductStatsDto
            {
                ProductId = stats.ProductId,
                AverageRating = Math.Round(stats.AverageRating, 1), // Округляємо до 1 знаку (напр. 4.5)
                ReviewsCount = stats.Count
            };
        }
    }
}