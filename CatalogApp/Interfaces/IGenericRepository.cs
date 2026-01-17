using CatalogApp.Interfaces;

namespace CatalogApp.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id); // Додали ?
        Task<IReadOnlyList<T>> ListAllAsync();
        
        Task<T?> GetEntityWithSpec(ISpecification<T> spec); // Додали ?
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> SaveAllAsync();
    }
}