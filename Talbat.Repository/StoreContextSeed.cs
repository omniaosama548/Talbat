using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talbat.Core.Entites;
using Talbat.Core.Entites.Order_Agg;
using Talbat.Repository.Data;

namespace Talbat.Repository
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            //productBrand Seed
            if(!dbContext.ProductBrands.Any())
            {
                var BrandData = File.ReadAllText("../Talbat.Repository/Data/Data Seed/brands.json");
                var Brands=JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);
                if (Brands?.Count> 0)
                {
                    foreach (var Brand in Brands)
                    {
                        await dbContext.Set<ProductBrand>().AddAsync(Brand);
                    }
                    await dbContext.SaveChangesAsync(); 
                }
            }
            //productType Seed
            if(!dbContext.ProductTypes.Any())
            {
                var TypesData = File.ReadAllText("../Talbat.Repository/Data/Data Seed/types.json");
                var Types=JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                if (Types?.Count> 0)
                {
                    foreach (var Type in Types)
                    {
                        await dbContext.Set<ProductType>().AddAsync(Type);  
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            //Product Seed
            if (!dbContext.Products.Any())
            {
                var productData = File.ReadAllText("../Talbat.Repository/Data/Data Seed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);
                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    {
                        await dbContext.Set<Product>().AddAsync(product);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            //delivery Method seed
            if (!dbContext.DeliveryMethods.Any())
            {
                var DeliveryData = File.ReadAllText("../Talbat.Repository/Data/Data Seed/delivery.json");
                var Methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);
                if (Methods?.Count > 0)
                {
                    foreach (var method in Methods)
                    {
                        await dbContext.Set<DeliveryMethod>().AddAsync(method);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}


