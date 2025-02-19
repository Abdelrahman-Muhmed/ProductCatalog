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
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime startDate { get; set; } = DateTime.Today;

        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public int? Duration { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
		public int BrandId { get; set; }
     
        [Required(ErrorMessage = "Category is required.")]
		public int CategoryId { get; set; }

        [Required(ErrorMessage = "Picture is required.")]
        [ValidateFile(new string[] { ".jpg", ".jpeg", ".png" }, 1)]
		public IFormFile? ImageFile { get; set; }
		//public string? ExistingImagePath { get; set; }
	}
}
