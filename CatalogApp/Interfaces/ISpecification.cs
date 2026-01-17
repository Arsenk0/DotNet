using System.Linq.Expressions;

namespace CatalogApp.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; } // Додали ?
        List<Expression<Func<T, object>>> Includes { get; }
        Expression<Func<T, object>>? OrderBy { get; } // Додали ?
        Expression<Func<T, object>>? OrderByDescending { get; } // Додали ?
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}