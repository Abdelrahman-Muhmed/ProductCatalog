using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductCatalog_DAL.Models.IdentityModel;
using ProductCatalog_DAL.Models.Product;
using System.Reflection;

namespace ProductCatalog_DAL.Prsistence.Data
{
    public class ProductContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // Based on Reflaction 

        }

        public DbSet<Products> Product { get; set; }
        public DbSet<ProductBrand> ProductBrand { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
    }
}
