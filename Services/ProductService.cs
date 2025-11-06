using MiniCrmApi.Models;
using MiniCrmApi.Repositories;

namespace MiniCrmApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryService categoryService;

        public ProductService(IProductRepository productRepository, ICategoryService categoryService)
        {
            this.productRepository = productRepository;
            this.categoryService = categoryService;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            List<Product> productList = await productRepository.GetAllProductsAsync();
            return productList;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            Product product = await productRepository.GetProductByIdAsync(id);

            if(product == null)
            {
                throw new KeyNotFoundException($"There isn't product with this id:{id}");
            }

            return product;
        }

        public async Task AddProductAsync(Product product)
        {
            if(product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null");
            }

            await categoryService.GetCategoryByIdAsync(product.CategoryId);

            await productRepository.AddProductAsync(product);
        }

        public async Task UpdateProductAsync(int id, Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null");
            }

            Product dbProduct = await GetProductByIdAsync(id);

            if (product.Name != null)
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

            if (product.Price >= 0)
            {
                dbProduct.Price = product.Price;
            }

            if (product.CategoryId > 0)
            {
                await categoryService.GetCategoryByIdAsync(product.CategoryId);
                dbProduct.CategoryId = product.CategoryId;
            }

            await productRepository.UpdateProductAsync(dbProduct);
        }

        public async Task DeleteProductAsync(int id)
        {
            Product product = await GetProductByIdAsync(id);
            await productRepository.DeleteProductAsync(product);
        }
    }
}
