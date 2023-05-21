using Catalog.Models;

namespace Catalog.Data.Interfaces
{
    public interface IItemRepository
    {
        Task<IList<Item>> GetItemsAsync(int categoryId, int pageNumber, int pageSize);

        Task<Item> GetItemAsync(int id);

        Task<Item> AddItemAsync(Item item);

        Task<Item> UpdateItemAsync(int id, Item item);

        Task DeleteItemAsync(int id);
    }
}
