using Xunit;
using ProductCatalog_DAL.IRepository;
using ProductCatalog_DAL.Models.Product;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ProductCatalog_DAL.Prsistence.Repository;
using System;
using ProductCatalog_DAL.Prsistence.Data;

public class ProductRepoTests
{
	private DbContextOptions<ProductContext> GetInMemoryOptions()
	{
		return new DbContextOptionsBuilder<ProductContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique database name for each test
			.Options;
	}

	[Fact]
	public async Task GetAllProductsAsync_ReturnsAllProducts()
	{
		var options = GetInMemoryOptions();
		using (var context = new ProductContext(options))
		{
			context.Product.AddRange(
				new Products { Id = 1, Name = "Product 1", Description = "Description 1" },
				new Products { Id = 2, Name = "Product 2", Description = "Description 2" }
			);
			await context.SaveChangesAsync();
		}

		using (var context = new ProductContext(options))
		{
			var repo = new ProductRepo(context);

		
			var products = await repo.GetAllProductsAsync();

			
			Assert.NotNull(products);
			Assert.Equal(2, products.Count);
		}
	}

	[Fact]
	public async Task GetAsync_ReturnsProduct_WhenProductExists()
	{
		var options = GetInMemoryOptions();
		using (var context = new ProductContext(options))
		{
			context.Product.Add(new Products { Id = 1, Name = "Product 1", Description = "Description 1" });
			await context.SaveChangesAsync();
		}

		using (var context = new ProductContext(options))
		{
			var repo = new ProductRepo(context);

			
			var product = await repo.GetAsync(1);

			
			Assert.NotNull(product);
			Assert.Equal(1, product.Id);
			Assert.Equal("Product 1", product.Name);
		}
	}

	[Fact]
	public async Task GetAsync_ReturnsNull_WhenProductDoesNotExist()
	{
	
		var options = GetInMemoryOptions();
		using (var context = new ProductContext(options))
		{
			var repo = new ProductRepo(context);

			var product = await repo.GetAsync(99); 

			
			Assert.Null(product);
		}
	}
}
