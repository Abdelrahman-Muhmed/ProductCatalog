using AutoMapper;
using Microsoft.Extensions.Logging;
using ProductCatalog_BLL.DTOs;
using ProductCatalog_BLL.IService;
using ProductCatalog_DAL.IRepository;
using ProductCatalog_DAL.Models.Product;
using ProductCatalog_DAL.Prsistence.Repository;
using ProductCatalog_DAL.Repository;

namespace ProductCatalog_Service.ServiceRepo
{
    public class ProductServic : IProductService
    {


        private readonly IProductRepo _productRepo;
		private readonly IUnitOfWork _unitOfWork;


		public ProductServic(IProductRepo productRepo, IUnitOfWork unitOfWork)
        {
            _productRepo = productRepo;
            _unitOfWork = unitOfWork;


		}
        public async Task<string> AddPicture(ProductDto productDto)
        {
            string filePath = "";
            if (productDto.ImageFile != null && productDto.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine("wwwroot", "Images/Products");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(productDto.ImageFile.FileName);
                string fullPath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await productDto.ImageFile.CopyToAsync(stream);
                }

                filePath = uniqueFileName;
            }


            return filePath;

        }

	
		public async Task<string> UpdatePicture(ProductDto productDto, Products product)
        {
            string filePath = "";
            if (product != null)
            {
                filePath = product.PictureUrl;
                if (productDto.ImageFile != null && productDto.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine("wwwroot", "Images/Products");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid() + Path.GetExtension(productDto.ImageFile.FileName);
                    string fullPath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await productDto.ImageFile.CopyToAsync(stream);
                    }

                    filePath = uniqueFileName;
                }


            }
            return filePath;

        }


		public async Task AddProductAsync(Products product)
		{
			var repository = _unitOfWork.Repository<Products>();
			repository.Add(product);
			await _unitOfWork.CompleteAsync();
		}

		public async Task<bool> DeleteProductAsync(int id)
		{
			var repository = _unitOfWork.Repository<Products>();
			var product = await repository.GetAsync(id);

			if (product != null)
			{
				repository.Delete(product);
				await _unitOfWork.CompleteAsync();
				return true; // Return true if the product is deleted successfully.
			}

			return false; // Return false if the product is not found.
		}

		public async Task UpdateProductAsync(Products product)
		{
			var repository = _unitOfWork.Repository<Products>();
			repository.Update(product);
			await _unitOfWork.CompleteAsync();
		}
	}
}
