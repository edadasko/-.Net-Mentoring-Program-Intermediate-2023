using Catalog.Models;

namespace Catalog.Data.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IList<Category>> GetCategoriesAsync();

        Task<Category> GetCategoryAsync(int id);

        Task<Category> AddCategoryAsync(Category category);

        Task<Category> UpdateCategoryAsync(int id, Category category);

        Task DeleteCategoryAsync(int id);
    }
}
