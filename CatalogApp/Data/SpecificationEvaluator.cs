using CatalogApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogApp.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : class
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            // 1. Фільтрація (Where)
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            // 2. Включення зв'язаних даних (Include)
            // Це замінює ручне написання .Include(x => x.Category)
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            // 3. Сортування
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            // 4. Пагінація
            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            return query;
        }
    }
}