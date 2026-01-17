using System.Data;
using CatalogApp.AdoDapper.Data.Repositories;

namespace CatalogApp.AdoDapper.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        OrderRepository Orders { get; }
        ProductRepository Products { get; }
        
        IDbConnection Connection { get; }
        IDbTransaction? Transaction { get; } 
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}