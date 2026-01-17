using MediatR;

namespace CatalogApp.Mongo.Features.Reviews.Queries.GetProductStats
{
    public class GetProductStatsQuery : IRequest<ProductStatsDto>
    {
        public int ProductId { get; set; }

        public GetProductStatsQuery(int productId)
        {
            ProductId = productId;
        }
    }
}