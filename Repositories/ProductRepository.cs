using Microsoft.EntityFrameworkCore;
using MiniCrmApi.Data;
using MiniCrmApi.Models;

namespace MiniCrmApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MiniCrmContext context;

        public ProductRepository(MiniCrmContext context)
        {
            this.context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            List<Product> products = await context.Products.ToListAsync();
            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            Product? product = await context.Products.FindAsync(id);
            return product;
        }

        public async Task AddProductAsync(Product product)
        {
            await context.Products.AddAsync(product);
        }

        public Task UpdateProductAsync(Product product)
        {
            context.Products.Update(product);
            return Task.CompletedTask;
        }

        public Task DeleteProductAsync(Product product)
        {
            context.Products.Remove(product);
            return Task.CompletedTask;
        }
    }
}
