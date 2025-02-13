using ProductCatalog_DAL.Models;

namespace ProductCatalog_DAL.IRepository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {

        Task<T?> GetAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();

		Task<T> AddAsync(T entity);

		Task<T> DeleteAsync(int id);

		Task<T> UpdateAsync(T entity);

		//void Add(T entity);

		//void Update(T entity);

		//void Delete(T entity);




	}
}
