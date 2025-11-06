using MiniCrmApi.Models;
using MiniCrmApi.Repositories;

namespace MiniCrmApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            List<Category> categoryList = await categoryRepository.GetAllCategoriesAsync();
            return categoryList;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            Category category = await categoryRepository.GetCategoryByIdAsync(id);
            
            if(category == null)
            {
                throw new KeyNotFoundException($"There is no category for this id:{id}");
            }
            
            return category;
        }

        public async Task AddCategoryAsync(Category category)
        {
            if(category == null)
            {
                throw new ArgumentNullException(nameof(category),"Category cannot be null");
            }

            await categoryRepository.AddCategoryAsync(category);
        }

        public async Task UpdateCategoryAsync(int id, Category category)
        {
            Category dbCategory = await GetCategoryByIdAsync(id);
            
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category), "Category cannot be null");
            }

            if(category.Name != null)
            {
                dbCategory.Name = category.Name;
            }

            await categoryRepository.UpdateCategoryAsync(dbCategory);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            Category category = await GetCategoryByIdAsync(id);

            await categoryRepository.DeleteCategoryAsync(category);
        }
    }
}
