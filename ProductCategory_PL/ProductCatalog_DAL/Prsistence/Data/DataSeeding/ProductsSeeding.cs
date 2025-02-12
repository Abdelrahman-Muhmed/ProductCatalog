using ProductCatalog_DAL.Models.Product;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductCatalog_DAL.Prsistence.Data.DataSeeding
{
	public static class ProductsSeeding
	{
		public async static Task SeedAsync(ProductContext dbContext, int userId, DateTime creationDate)
		{
			var brands = File.ReadAllText("../ProductCatalog_DAL/Prsistence/Data/DataSeeding/brands.json");
			var category = File.ReadAllText("../ProductCatalog_DAL/Prsistence/Data/DataSeeding/category.json");
			var product = File.ReadAllText("../ProductCatalog_DAL/Prsistence/Data/DataSeeding/products.json");

			var brandsData = JsonSerializer.Deserialize<List<ProductBrand>>(brands);
			var categoryData = JsonSerializer.Deserialize<List<ProductCategory>>(category);
			var productData = JsonSerializer.Deserialize<List<Products>>(product);

			if (dbContext.ProductBrand.Count() == 0)
			{
				if (brandsData?.Count() > 0)
				{
					foreach (var brand in brandsData)
					{
						dbContext.Set<ProductBrand>().Add(brand);
					}
				}

				await dbContext.SaveChangesAsync();
			}

			if (dbContext.ProductCategory.Count() == 0)
			{
				if (categoryData?.Count() > 0)
				{
					foreach (var categoryItem in categoryData)
					{
						dbContext.Set<ProductCategory>().Add(categoryItem);
					}
				}
				await dbContext.SaveChangesAsync();
			}

			if (dbContext.Product.Count() == 0)
			{
				if (productData?.Count() > 0)
				{
					foreach (var productItem in productData)
					{
						productItem.createdBy = userId;
						productItem.creationDate = creationDate;
						dbContext.Set<Products>().Add(productItem);
					}
				}
				await dbContext.SaveChangesAsync();
			}
		}
	}
}