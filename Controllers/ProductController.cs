using Microsoft.AspNetCore.Mvc;
using MiniCrmApi.Dtos;
using MiniCrmApi.Models;
using MiniCrmApi.Services;

namespace MiniCrmApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            List<Product> dbProductList = await productService.GetAllProductsAsync();

            List<ProductResponseDto> productList = dbProductList.Select(p =>
                new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryId = p.CategoryId,
                }).ToList();

            return Ok(productList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            Product dbProduct = await productService.GetProductByIdAsync(id);

            ProductResponseDto product = new ProductResponseDto
            {
                Id = dbProduct.Id,
                Name = dbProduct.Name,
                Description = dbProduct.Description,
                Price = dbProduct.Price,
                StockQuantity = dbProduct.StockQuantity,
                CategoryId = dbProduct.CategoryId
            };

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductRequestDto productRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = new Product
            {
                Name = productRequest.Name,
                Description = productRequest.Description,
                Price = productRequest.Price,
                StockQuantity = productRequest.StockQuantity,
                CategoryId = productRequest.CategoryId
            };

            await productService.AddProductAsync(product);

            ProductResponseDto productResponse = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId
            };

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, productResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequestDto productRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = new Product
            {
                Name = productRequest.Name,
                Description = productRequest.Description,
                Price = productRequest.Price,
                StockQuantity = productRequest.StockQuantity,
                CategoryId = productRequest.CategoryId
            };

            await productService.UpdateProductAsync(id, product);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
