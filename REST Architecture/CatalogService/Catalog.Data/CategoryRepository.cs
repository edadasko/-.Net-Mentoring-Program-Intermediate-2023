using Catalog.Data.Interfaces;
using Catalog.Models;
using Catalog.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Catalog.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CatalogContext _context;

        public CategoryRepository(CatalogContext context)
        {
            _context = context;
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var categoryToDelete = await _context.Categories.FindAsync(id) 
                ?? throw new ApiException(HttpStatusCode.NotFound);

            var itemsToDelete = _context.Items.Where(item => item.CategoryId == id);
            _context.Items.RemoveRange(itemsToDelete);
            _context.Categories.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Category>> GetCategoriesAsync() =>
            await _context.Categories.ToListAsync();

        public async Task<Category> GetCategoryAsync(int id) => 
            await _context.Categories.FindAsync(id);

        public async Task<Category> UpdateCategoryAsync(int id, Category category)
        {
            Category categoryToUpdate = await _context.Categories.FindAsync(id) 
                ?? throw new ApiException(HttpStatusCode.NotFound);

            categoryToUpdate.Name = category.Name;
            categoryToUpdate.Description = category.Name;

            await _context.SaveChangesAsync();
            return categoryToUpdate;
        }
    }
}
