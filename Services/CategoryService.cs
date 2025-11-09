using Microsoft.EntityFrameworkCore.Storage;
using MiniCrmApi.Data;
using MiniCrmApi.Models;
using MiniCrmApi.Repositories;

namespace MiniCrmApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly MiniCrmContext context;

        public CategoryService(ICategoryRepository categoryRepository, MiniCrmContext context)
        {
            this.categoryRepository = categoryRepository;
            this.context = context;
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

            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await categoryRepository.AddCategoryAsync(category);
                await context.SaveChangesAsync();

                await transaction.CommitAsync(); // Her şey başarılı ise commit edecek.
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); // Hata olursa rollback yapacak.
                throw; // ExceptionMiddleware yakalayacak
            }

        }

        public async Task UpdateCategoryAsync(int id, Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category), "Category cannot be null");
            }

            Category dbCategory = await GetCategoryByIdAsync(id);

            if(category.Name != null)
            {
                dbCategory.Name = category.Name;
            }

            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await categoryRepository.UpdateCategoryAsync(dbCategory);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            Category category = await GetCategoryByIdAsync(id);

            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await categoryRepository.DeleteCategoryAsync(category);
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
