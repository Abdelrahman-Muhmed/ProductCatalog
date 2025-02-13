

namespace ProductCatalog_DAL.Models.Product
{
    public class ProductBrand : BaseEntity
    {
        public string Name { get; set; } = null!;

        //Handle Nagational Prop By Fluint Api inside Prsistence => Data => Config Folder 
    }
}
