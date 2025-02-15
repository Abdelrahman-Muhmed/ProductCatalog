using Microsoft.AspNetCore.Http;
using ProductCatalog_BLL.Helpers.AllowedExtention;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalog_BLL.DTOs
{
    public class ProductDto
	{
        public int? Id { get; set; }


		[Required(ErrorMessage = "Name is required.")]
		[StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
		public string Name { get; set; } = null!;

		[Required(ErrorMessage = "Description is required.")]
		[StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
		public string Description { get; set; } = null!;

		//[Required(ErrorMessage = "Picture URL is required.")]
		//[Url(ErrorMessage = "Picture URL must be a valid URL.")]
		//public string? PictureUrl { get; set; } = null!;

		[Required(ErrorMessage = "Price is required.")]
		public decimal Price { get; set; }

		public DateTime startDate { get; set; } = DateTime.Today;
        public int? Duration { get; set; }

        [Required(ErrorMessage = "BrandId is required.")]
		public int BrandId { get; set; }
     
        [Required(ErrorMessage = "CategoryId is required.")]
		public int CategoryId { get; set; }

		[ValidateFile(new string[] { ".jpg", ".jpeg", ".png" }, 1, ErrorMessage = "Invalid file. Only .jpg, .jpeg, .png files under 1 MB are allowed.")]
		public IFormFile? ImageFile { get; set; }
		//public string? ExistingImagePath { get; set; }
	}
}
