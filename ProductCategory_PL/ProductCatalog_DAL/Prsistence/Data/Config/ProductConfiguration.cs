using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog_DAL.Models.Product;

namespace ProductCatalog_DAL.Prsistence.Data.Config
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(p => p.PictureUrl).IsRequired();

            builder.Property(p => p.Price).HasColumnType("decimal(18 , 2)");

            builder.HasOne(x => x.ProductBrand)
                   .WithMany()
                   .HasForeignKey(x => x.BrandId);

            builder.HasOne(x => x.ProductCategory)
                   .WithMany()
                   .HasForeignKey(x => x.CategoryId);
        }
    }
}
