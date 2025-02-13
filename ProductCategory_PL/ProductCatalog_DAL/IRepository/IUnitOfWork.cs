using ProductCatalog_DAL.Models;

namespace ProductCatalog_DAL.IRepository
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        //Make This prop as a Methode 
        IGenericRepository<T> Repository<T>() where T : BaseEntity;  // So when i path any Table here he will create The Object What i need When i ask 
        Task<int> CompleteAsync();
    }
}
