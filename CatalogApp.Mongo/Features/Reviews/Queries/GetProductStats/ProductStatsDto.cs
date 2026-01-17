namespace CatalogApp.Mongo.Features.Reviews.Queries.GetProductStats
{
    public class ProductStatsDto
    {
        public int ProductId { get; set; }
        public double AverageRating { get; set; }
        public int ReviewsCount { get; set; }
    }
}