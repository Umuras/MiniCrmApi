using Microsoft.AspNetCore.Mvc;
using MiniCrmApi.Dtos;
using MiniCrmApi.Models;
using MiniCrmApi.Services;
using System.Threading.Tasks;

namespace MiniCrmApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            List<Category> categoryList = await categoryService.GetAllCategoriesAsync();

            //Burada Select ile tüm categoryListteki category nesnelerini dönüyoruz, o nesneler üzerinden CategoryResponseDto
            //türünde nesne üretip hepsinin bir liste şeklinde oluşmasını sağlıyoruz.
            List<CategoryResponseDto> results = categoryList.Select(category => new CategoryResponseDto
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
            }).ToList();

            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            Category dbCategory = await categoryService.GetCategoryByIdAsync(id);
            CategoryResponseDto category = new CategoryResponseDto
            {
                CategoryId = dbCategory.Id,
                CategoryName = dbCategory.Name,
            };

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequestDto categoryDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = new Category
            {
                Name = categoryDto.CategoryName
            };

            await categoryService.AddCategoryAsync(category);

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, categoryDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequestDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = new Category
            {
                Name = categoryDto.CategoryName
            };

            await categoryService.UpdateCategoryAsync(id, category);

            return Ok("Category updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
