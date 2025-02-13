using ProductCatalog_BLL.IService;
using ProductCatalog_DAL.IRepository;
using ProductCatalog_DAL.Models.Product;

namespace ProductCatalog_Service.ServiceRepo
{
    public class ProductServic : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;
		public ProductServic(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Task<IReadOnlyList<Products>> GetAllProductAsync()
		{
			var products = _unitOfWork.Repository<Products>().GetAllAsync();

			return products;
		}

		public Task<Products> GetProductAsync(int id)
		{
			var product = _unitOfWork.Repository<Products>().GetAsync(id);

			return product;
		}

		public Task<IReadOnlyList<ProductBrand>> GetProductBrandAsync()
		{
			var brands = _unitOfWork.Repository<ProductBrand>().GetAllAsync();
			return brands;
		}

		public Task<IReadOnlyList<ProductCategory>> GetProductCategoryAsync()
		{
			var categors = _unitOfWork.Repository<ProductCategory>().GetAllAsync();
			return categors;
		}
	
	}
}
