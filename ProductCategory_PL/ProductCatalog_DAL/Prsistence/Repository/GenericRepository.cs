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

		public async Task<T> AddAsync(T entity)
		{
			await _dbContext.AddAsync(entity);
			return entity;
		}

		public async Task<T> DeleteAsync(int id)
		{
			var entity = await GetAsync(id);
			if (entity != null)
			{
				_dbContext.Set<T>().Remove(entity);
			}
			return entity;
		}

		public async Task<T> UpdateAsync(T entity)
		{
			_dbContext.Set<T>().Update(entity);
			return entity;
		}




	}
}
