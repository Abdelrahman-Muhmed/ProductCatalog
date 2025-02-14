using AutoMapper;
using ProductCatalog_BLL.DTOs;
using ProductCatalog_BLL.IService;
using ProductCatalog_DAL.IRepository;
using ProductCatalog_DAL.Models.Product;

namespace ProductCatalog_Service.ServiceRepo
{
    public class ProductServic : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public ProductServic(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public Task<IReadOnlyList<Products>> GetAllProductAsync()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<object>> GetAllProductsTransformedAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Products> GetProductAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<ProductBrand>> GetProductBrandAsync()
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<ProductCategory>> GetProductCategoryAsync()
		{
			throw new NotImplementedException();
		}

		//public async Task<IEnumerable<object>> GetAllProductsTransformedAsync()
		//{
		//	var getProducts = await GetAllProductAsync();
		//	var data = _mapper.Map<IReadOnlyList<Products>, IReadOnlyList<ProductReturnDto>>(getProducts);

		//	int? n = null;
		//	return data.Select(item => new
		//	{
		//		ProductId = item.Id,
		//		ProductName = item.Name,
		//		price = item.Price,
		//		productCategory = item.CategoryName,
		//		productDescription = item.Description,
		//		productBrand = item.ProductBrand,
		//		Action = n,
		//		Num = n
		//	});
		//}

		//public Task<IReadOnlyList<Products>> GetAllProductAsync()
		//{
		//	var products = _unitOfWork.Repository<Products>().GetAllAsync();

		//	return products;
		//}

		//public Task<Products> GetProductAsync(int id)
		//{
		//	var product = _unitOfWork.Repository<Products>().GetAsync(id);

		//	return product;
		//}


	}
}
