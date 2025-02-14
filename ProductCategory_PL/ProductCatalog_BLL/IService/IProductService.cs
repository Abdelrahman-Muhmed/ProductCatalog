using ProductCatalog_DAL.Models.Product;

namespace ProductCatalog_BLL.IService
{
    public interface IProductService
	{
		Task<IReadOnlyList<Products>> GetAllProductAsync();
		Task<Products> GetProductAsync(int id);
		Task<IReadOnlyList<ProductBrand>> GetProductBrandAsync();
		Task<IReadOnlyList<ProductCategory>> GetProductCategoryAsync();


	     Task<IEnumerable<object>> GetAllProductsTransformedAsync();

	}
}
