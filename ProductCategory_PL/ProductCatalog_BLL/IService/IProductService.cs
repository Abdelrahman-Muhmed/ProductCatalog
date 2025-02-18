using Microsoft.EntityFrameworkCore.Update.Internal;
using ProductCatalog_BLL.DTOs;
using ProductCatalog_DAL.Models.Product;

namespace ProductCatalog_BLL.IService
{
    public interface IProductService
	{

        Task<string> UpdatePicture(ProductDto productDto, Products products);
        Task<string> AddPicture(ProductDto productDto);

		Task AddProductAsync(Products product);
		Task UpdateProductAsync(Products product);
		Task<bool> DeleteProductAsync(int id);
	}
}
