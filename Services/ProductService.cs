using Microsoft.EntityFrameworkCore.Storage;
using MiniCrmApi.Data;
using MiniCrmApi.Models;
using MiniCrmApi.Repositories;

namespace MiniCrmApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryService categoryService;
        private readonly MiniCrmContext context;

        public ProductService(IProductRepository productRepository, ICategoryService categoryService, MiniCrmContext context)
        {
            this.productRepository = productRepository;
            this.categoryService = categoryService;
            this.context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            List<Product> productList = await productRepository.GetAllProductsAsync();
            return productList;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            Product product = await productRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException($"There isn't product with this id:{id}");
            }

            return product;
        }

        public async Task AddProductAsync(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null");
            }

            await categoryService.GetCategoryByIdAsync(product.CategoryId);

            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await productRepository.AddProductAsync(product);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        public async Task UpdateProductAsync(int id, Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null");
            }

            Product dbProduct = await GetProductByIdAsync(id);

            if (product.Name != null && product.Name != string.Empty)
            {
                dbProduct.Name = product.Name;
            }

            if (product.Description != null)
            {
                dbProduct.Description = product.Description;
            }

            if (product.StockQuantity >= 0)
            {
                dbProduct.StockQuantity = product.StockQuantity;
            }
            else
            {
                throw new ArgumentException("Stock quantity cannot be negative");
            }

            if (product.Price >= 0)
            {
                dbProduct.Price = product.Price;
            }
            else
            {
                throw new ArgumentException("Price quantity cannot be negative");
            }

            if (product.CategoryId > 0)
            {
                await categoryService.GetCategoryByIdAsync(product.CategoryId);
                dbProduct.CategoryId = product.CategoryId;
            }

            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await productRepository.UpdateProductAsync(dbProduct);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        public async Task DeleteProductAsync(int id)
        {
            Product product = await GetProductByIdAsync(id);

            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await productRepository.DeleteProductAsync(product);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
