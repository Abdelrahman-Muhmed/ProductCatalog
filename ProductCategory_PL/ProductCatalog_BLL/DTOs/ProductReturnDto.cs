namespace ProductCatalog_BLL.DTOs
{
    public class ProductReturnDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; } 
        public string? Description { get; set; } 
        public string? PictureUrl { get; set; }
        public decimal Price { get; set; }
        public DateTime? StartDate { get; set; }
        public string? ProductBrand { get; set; }

        public string? CategoryName { get; set; }
    }
}
