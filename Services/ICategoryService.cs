using MiniCrmApi.Models;

namespace MiniCrmApi.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(int id, Category category);
        Task DeleteCategoryAsync(int id);
    }
}
