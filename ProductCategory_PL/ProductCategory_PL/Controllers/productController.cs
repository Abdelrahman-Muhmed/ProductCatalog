using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductCatalog_BLL.DTOs;
using ProductCatalog_BLL.IService;
using ProductCatalog_DAL.IRepository;
using ProductCatalog_DAL.Models.Product;
using System.Data;
using System.Security.Claims;
namespace ProductCatalog_PL.Controllers
{
    public class productController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IProductService _productService;
		private readonly IGenericRepository<Products> _genericRepository;
		private readonly IGenericRepository<ProductCategory> _genericCategoryRepository;
		private readonly IGenericRepository<ProductBrand> _genericBrandRepository;

		private readonly IProductRepo _productRepo;
		public productController (IProductRepo productRepo, IMapper mapper, IProductService productService, IGenericRepository<Products> genericRepository, IGenericRepository<ProductCategory> genericCategoryRepository, IGenericRepository<ProductBrand> genericBrandRepository)
		{
			_mapper = mapper;
			_productService = productService;
			_genericRepository = genericRepository;
            _productRepo = productRepo;
			_genericCategoryRepository = genericCategoryRepository;
            _genericBrandRepository = genericBrandRepository;

		}
		public IActionResult Home()
		{
			return View();
			
		}

     
		public IActionResult GetAll()
		{

			var dataRows = _productRepo.GetAllProductsAsync();
			return Json(new { data = dataRows });
		}


		[HttpGet("{id}")]
		public async Task<ActionResult<ProductReturnDto>> GetProductAsync(int id)
		{
			var product = await _productService.GetProductAsync(id);
			if (product == null)
				return NotFound();
			var ProductMapp = _mapper.Map<Products, ProductReturnDto>(product);
			return Ok(ProductMapp);

		}


		[HttpGet]
		public async Task<IActionResult> Add()
		{
            //var selecCategorytListItems = await _genericCategoryRepository.GetAllAsync<ProductCategory>();
            //ViewData["BrandId"] = new SelectList(selecCategorytListItems, "Id", "Name");

            var selecBrandtListItems = await _productService.GetProductBrandAsync();
            ViewData["CategoryId"] = new SelectList(selecBrandtListItems, "Id", "Name");

            return View(new ProductDto());
		}

        [HttpPost]
        public async Task<IActionResult> Add(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Save the file and get its path
                    string filePath = null;
                    if (productDto.ImageFile != null && productDto.ImageFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine("wwwroot", "uploads");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        filePath = Path.Combine(uploadsFolder, Guid.NewGuid() + Path.GetExtension(productDto.ImageFile.FileName));
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await productDto.ImageFile.CopyToAsync(stream);
                        }
                    }

                    // Map DTO to entity
                    Products product = _mapper.Map<Products>(productDto);
                    product.PictureUrl = filePath;
					product.creationDate = DateTime.Now;

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
					if(userId != null)
					{
						product.createdBy = int.Parse(userId);

                    }

                    // Save to database
                    await _genericRepository.AddAsync(product);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding");
                }
            }
            return BadRequest("Invalid data");
        }

        //[HttpPut]
        //public async Task<IActionResult> Update(ProductDto productDto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var product = await _productService.GetProductAsync();
        //            if (product == null)
        //            {
        //                return NotFound("Product not found.");
        //            }

                  
        //            string filePath = product.PictureUrl; 
        //            if (productDto.ImageFile != null && productDto.ImageFile.Length > 0)
        //            {
        //                string uploadsFolder = Path.Combine("wwwroot", "uploads");
        //                if (!Directory.Exists(uploadsFolder))
        //                {
        //                    Directory.CreateDirectory(uploadsFolder);
        //                }

        //                filePath = Path.Combine(uploadsFolder, Guid.NewGuid() + Path.GetExtension(productDto.ImageFile.FileName));
        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await productDto.ImageFile.CopyToAsync(stream);
        //                }
        //            }

        //            product = _mapper.Map(productDto, product);
        //            product.PictureUrl = filePath;  
        //            product.UpdateDate = DateTime.Now;  

        //            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //            if (userId != null)
        //            {
        //                product.UpdatedBy = int.Parse(userId);
        //            }

        //            // Save the updated product to the database
        //            await _genericRepository.UpdateAsync(product);

        //            return Ok("Update Successfully");
        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating: {ex.Message}");
        //        }
        //    }

        //    return BadRequest("Invalid Data");
        //}


        [HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var delete = await _genericRepository.DeleteAsync(id);
					if (delete is null)
						return NotFound("Not Found This Course");

					return Ok("Deleted SuccessFully");
				}
				catch
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred While Deleting");
				}
			}
			return BadRequest("Invalid Data");
		}

	}

}
