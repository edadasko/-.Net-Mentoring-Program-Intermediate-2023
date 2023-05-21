using Catalog.Data.Interfaces;
using Catalog.Models;
using Catalog.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;

        public ItemsController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<Category>>> Get(
            [FromQuery] int categoryId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _itemRepository.GetItemsAsync(categoryId, pageNumber, pageSize);
            return Ok(items);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IList<Category>>> GetById(int id)
        {
            var item = await _itemRepository.GetItemAsync(id);

            return item == null
                ? throw new ApiException(HttpStatusCode.NotFound) 
                : Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Category>> Create([FromBody] Item item)
        {
            var createdItem = await _itemRepository.AddItemAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = createdItem.Id }, createdItem);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> Update(int id, [FromBody] Item item)
        {
            var updatedCategory = await _itemRepository.UpdateItemAsync(id, item);
            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _itemRepository.DeleteItemAsync(id);
            return NoContent();
        }
    }
}
