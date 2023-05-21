using Catalog.Data.Interfaces;
using Catalog.Models;
using Catalog.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Catalog.Data
{
    public class ItemRepository : IItemRepository
    {
        private readonly CatalogContext _context;

        public ItemRepository(CatalogContext context)
        {
            _context = context;
        }

        public async Task<Item> AddItemAsync(Item item)
        {
            if (await _context.Categories.FindAsync(item.CategoryId) is null)
            {
                throw new ApiException(HttpStatusCode.NotFound);
            }

            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task DeleteItemAsync(int id)
        {
            var itemToDelete = await _context.Items.FindAsync(id) 
                ?? throw new ApiException(HttpStatusCode.NotFound);

            _context.Items.Remove(itemToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<Item> GetItemAsync(int id) =>
            await _context.Items.FindAsync(id);

        public async Task<IList<Item>> GetItemsAsync(int categoryId, int pageNumber, int pageSize) =>
            await _context.Items
                .Where(item => item.CategoryId == categoryId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public async Task<Item> UpdateItemAsync(int id, Item item)
        {
            var itemToUpdate = await _context.Items.FindAsync(id) 
                ?? throw new ApiException(HttpStatusCode.NotFound);

            if (item.CategoryId != itemToUpdate.CategoryId)
            {
                if (await _context.Categories.FindAsync(item.CategoryId) is null)
                {
                    throw new ApiException(HttpStatusCode.NotFound);
                }
            }

            itemToUpdate.Name = item.Name;
            itemToUpdate.Description = item.Description;
            itemToUpdate.CategoryId = item.CategoryId;
            await _context.SaveChangesAsync();
            return itemToUpdate;
        }
    }
}
