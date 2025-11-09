using Microsoft.EntityFrameworkCore;
using MiniCrmApi.Data;
using MiniCrmApi.Models;

namespace MiniCrmApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MiniCrmContext context;

        public CategoryRepository(MiniCrmContext context)
        {
            this.context = context;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            List<Category> categories = await context.Categories.ToListAsync();
            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            Category? category = await context.Categories.FindAsync(id);
            return category;
        }

        public async Task AddCategoryAsync(Category category)
        {
           await context.Categories.AddAsync(category);
        }

        public Task UpdateCategoryAsync(Category category)
        {
            context.Categories.Update(category);
            return Task.CompletedTask;
        }

        public Task DeleteCategoryAsync(Category category)
        {
            context.Categories.Remove(category);
            return Task.CompletedTask;
        }
    }
}
