using AutoMapper;
using Microsoft.Extensions.Logging;
using ProductCatalog_BLL.DTOs;
using ProductCatalog_BLL.IService;
using ProductCatalog_DAL.IRepository;
using ProductCatalog_DAL.Models.Product;
using ProductCatalog_DAL.Prsistence.Repository;

namespace ProductCatalog_Service.ServiceRepo
{
    public class ProductServic : IProductService
    {


        private readonly IProductRepo _productRepo;


        public ProductServic(IProductRepo productRepo)
        {
            _productRepo = productRepo;

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


    }
}
