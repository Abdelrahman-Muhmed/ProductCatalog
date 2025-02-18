using Microsoft.EntityFrameworkCore;
using ProductCatalog_DAL.IRepository;
using ProductCatalog_DAL.Models;
using ProductCatalog_DAL.Prsistence.Data;

namespace ProductCatalog_DAL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ProductContext _dbContext;
	

		public GenericRepository(ProductContext dbContext)
        {
            _dbContext = dbContext;
			

		}

		public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
		}

		public async Task<T?> GetAsync(int? id)
		{
			return await _dbContext.Set<T>().FindAsync(id);
		}

		public void Add(T entity)
		 => _dbContext.Set<T>().Add(entity);

		public void Update(T entity)
		=> _dbContext.Set<T>().Update(entity);

		public void Delete(T entity)
		=> _dbContext.Set<T>().Remove(entity);




	}
}
