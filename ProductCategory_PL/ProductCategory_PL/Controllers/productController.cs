using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductCatalog_BLL.DTOs;
using ProductCatalog_BLL.Helpers.Encryption;
using ProductCatalog_BLL.IService;
using ProductCatalog_DAL.IRepository;
using ProductCatalog_DAL.Models.Product;
using ProductCatalog_Service.ServiceRepo;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
namespace ProductCatalog_PL.Controllers
{
    public class productController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Products> _genericRepository;
        private readonly IGenericRepository<ProductCategory> _genericCategoryRepository;
        private readonly IGenericRepository<ProductBrand> _genericBrandRepository;
        private readonly IProductRepo _productRepo;
        private readonly ILogger<productController> _logger;
        private readonly IProductService _productService;

        public productController(
            IMapper mapper, IProductRepo productRepo,
            IGenericRepository<Products> genericRepository,
            IGenericRepository<ProductCategory> genericCategoryRepository,
            IGenericRepository<ProductBrand> genericBrandRepository,
            ILogger<productController> logger,
             IProductService productService
            )
        {
            _mapper = mapper;
            _genericRepository = genericRepository;
            _productRepo = productRepo;
            _genericCategoryRepository = genericCategoryRepository;
            _genericBrandRepository = genericBrandRepository;
            _logger = logger;
            _productService = productService;


        }
        [Authorize(Roles = "Admin,Customer")]
        public IActionResult Home()
        {
            return View();

        }
        public IActionResult GetAll()
        {
            // Using Repository for products to get all products with specific columns
            var dataRows = _productRepo.GetAllProductsAsync();
            bool isAdmin = User.IsInRole("Admin");
            return Json(new { data = dataRows, role = isAdmin });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUpdate(string id)
        {
            ProductDto productDto = new ProductDto();
            if (!string.IsNullOrEmpty(id))
            {
				

				string decryptedId;
				try
				{
					decryptedId = EncryptionHelper.DecryptString(id);
				}
				catch
				{
					return BadRequest("Invalid product ID.");
				}

				if (!int.TryParse(decryptedId, out int productId))
				{
					return BadRequest("Invalid product ID.");
				}

				//using Repository for products to get all products with specific columns
				var product = await _productRepo.GetAsync(productId);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }
                if (product.ProductCategory != null && product.ProductBrand != null)
                {
                    productDto = _mapper.Map<ProductDto>(product);
                    productDto.Id = productId;
                    var selecCategorytListItemsUp = await _genericCategoryRepository.GetAllAsync();
                    ViewData["BrandId"] = new SelectList(selecCategorytListItemsUp, "Id", "Name", product.ProductCategory.Name);

                    var selecBrandtListItemsUp = await _genericBrandRepository.GetAllAsync();
                    ViewData["CategoryId"] = new SelectList(selecBrandtListItemsUp, "Id", "Name", product.ProductBrand.Name);
                }



            }
            else
            {
                var selecCategorytListItems = await _genericCategoryRepository.GetAllAsync();
                ViewData["BrandId"] = new SelectList(selecCategorytListItems, "Id", "Name");

                var selecBrandtListItems = await _genericBrandRepository.GetAllAsync();
                ViewData["CategoryId"] = new SelectList(selecBrandtListItems, "Id", "Name");
            }

            return View(productDto);
        }

		[HttpPost]
		[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUpdate(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                var selecCategorytListItemsUp = await _genericCategoryRepository.GetAllAsync();
                ViewData["CategoryId"] = new SelectList(selecCategorytListItemsUp, "Id", "Name", productDto.CategoryId);

                var selecBrandtListItemsUp = await _genericBrandRepository.GetAllAsync();
                ViewData["BrandId"] = new SelectList(selecBrandtListItemsUp, "Id", "Name", productDto.BrandId);
                return View(productDto);
            }

            if (productDto.Id != null)
            {
                var product = await _productRepo.GetAsync(productDto.Id);
                if (product != null)
                {
                    string filePath = await _productService.UpdatePicture(productDto, product);

                    product = _mapper.Map(productDto, product);
                    product.PictureUrl = filePath;
                    product.creationDate = DateTime.Now;

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId != null)
                    {
                        product.createdBy = int.Parse(userId);
                    }

					await _productService.UpdateProductAsync(product);

					return RedirectToAction("Home");
                }
                return NotFound("Product not found.");
            }
            else
            {
                string filePath = await _productService.AddPicture(productDto);
                Products product = _mapper.Map<Products>(productDto);
                product.PictureUrl = filePath;
                product.creationDate = DateTime.Now;

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != null)
                {
                    product.createdBy = int.Parse(userId);
                }
				await _productService.AddProductAsync(product);
				return RedirectToAction("Home");
            }


        }

        //Without Encrypt 
        //[HttpGet]
        //[Authorize(Roles = "Admin,Customer")]
        //public async Task<IActionResult> Details(int? id)
        //{
        //    var product = await _productRepo.GetAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound("Product not found.");
        //    }
        //    var ProductMapp = _mapper.Map<Products, ProductReturnDto>(product);
        //    return View(ProductMapp);

        //}

        //With Encrypt 
        [HttpGet]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Product not found.");
            }

            string decryptedId;
            try
            {
                decryptedId = EncryptionHelper.DecryptString(id);
            }
            catch
            {
                return BadRequest("Invalid product ID.");
            }

            if (!int.TryParse(decryptedId, out int productId))
            {
                return BadRequest("Invalid product ID.");
            }

            var product = await _productRepo.GetAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var productMapp = _mapper.Map<Products, ProductReturnDto>(product);
            return View(productMapp);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Product not found.");
            }

            string decryptedId;
            try
            {
                decryptedId = EncryptionHelper.DecryptString(id);
            }
            catch
            {
                return BadRequest("Invalid product ID.");
            }

            if (!int.TryParse(decryptedId, out int productId))
            {
                return BadRequest("Invalid product ID.");
            }


            try
            {
				bool isDeleted = await _productService.DeleteProductAsync(productId);
				if (!isDeleted)
					return Json(new { isValid = false, message = "Not Found This Product" });

                return Json(new { isValid = true, message = "Deleted Successfully" });
            }
            catch
            {
                return Json(new { isValid = false, message = "An error occurred while deleting" });
            }


		}



		//[HttpGet("{id}")]
		//[Authorize(Roles = "Admin")]
		//public async Task<ActionResult<ProductReturnDto>> GetProductAsync(int id)
		//{
		//	//using Repository for products to get all products with specific columns
		//	var product = await _productRepo.GetAsync(id);
		//	if (product == null)
		//		return NotFound();
		//	var ProductMapp = _mapper.Map<Products, ProductReturnDto>(product);
		//	return Ok(ProductMapp);

		//}
	}

}
