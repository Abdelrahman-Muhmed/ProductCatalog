using Microsoft.EntityFrameworkCore;
using ProductCatalog_DAL.IRepository;
using ProductCatalog_DAL.Models.Product;
using ProductCatalog_DAL.Prsistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProductCatalog_DAL.Prsistence.Repository
{

	public class ProductRepo : IProductRepo
	{
		private readonly ProductContext _dbContext;
		public ProductRepo(ProductContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<IReadOnlyList<Products>> GetAllProductsAsync()
		{
		
			var products = await _dbContext.Set<Products>()
				.Include(x => x.ProductBrand)
				.Include(x => x.ProductCategory)
				.AsNoTracking()
				.ToListAsync();

	
			return products;
		
		
		}


		public async Task<IReadOnlyList<ProductBrand>> GetAllProductBrandsAsync()
		{
			var productBrand = await _dbContext.Set<ProductBrand>().ToListAsync();

			return productBrand;
		}

		public async Task<IReadOnlyList<ProductCategory>> GetAllProductCategoryAsync()
		{
			var productCategory = await _dbContext.Set<ProductCategory>().ToListAsync();

			return productCategory;
		}

		Task<IReadOnlyList<ProductCategory>> IProductRepo.GetAllProductBrandsAsync()
		{
			throw new NotImplementedException();
		}

		Task<IReadOnlyList<ProductBrand>> IProductRepo.GetAllProductCategoryAsync()
		{
			throw new NotImplementedException();
		}
	}
}
