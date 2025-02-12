
using ProductCatalog_DAL.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog_DAL.IRepository
{
	public interface IProductRepo
	{
		Task<IReadOnlyList<Products>> GetAllProductsAsync();
		Task<Products?> GetAsync(int? id);

	}
}
