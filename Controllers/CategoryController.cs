using Microsoft.AspNetCore.Mvc;
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

            return Ok(categoryList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            Category category = await categoryService.GetCategoryByIdAsync(id);

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await categoryService.AddCategoryAsync(category);

            return CreatedAtAction(nameof(GetCategoryById), new { Id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await categoryService.UpdateCategoryAsync(id, category);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
