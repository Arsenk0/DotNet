using CatalogApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogApp.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly CatalogContext _context;

        public GenericRepository(CatalogContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(int id) // Додали ?
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetEntityWithSpec(ISpecification<T> spec) // Додали ?
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public void Add(T entity) => _context.Set<T>().Add(entity);
        public void Update(T entity) => _context.Set<T>().Attach(entity).State = EntityState.Modified;
        public void Delete(T entity) => _context.Set<T>().Remove(entity);
        public async Task<bool> SaveAllAsync() => await _context.SaveChangesAsync() > 0;

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }
    }
}