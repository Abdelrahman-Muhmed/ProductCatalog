
namespace ProductCatalog_DAL.Models.Product
{
    public class Products : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public int BrandId { get; set; } // FK ProductBrand
        public ProductBrand ProductBrand { get; set; } = null!; //Navigational Prop (One) ==> For Table ProductBrand
        public int CategoryId { get; set; } // FK ProdactCategory 
        public ProductCategory ProductCategory { get; set; } = null!; //Navigational Prop (One) ==> For Table ProdactCategory 

        //Handle FK By Fluint Api inside Prsistence => Data => Config Folder 
    }
}
