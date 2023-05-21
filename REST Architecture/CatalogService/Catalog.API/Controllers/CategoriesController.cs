using Catalog.Data.Interfaces;
using Catalog.Models;
using Catalog.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<Category>>> GetAll()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            return Ok(categories);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IList<Category>>> GetById(int id)
        {
            var category = await _categoryRepository.GetCategoryAsync(id);

            return category == null 
                ? throw new ApiException(HttpStatusCode.NotFound) 
                : Ok(category);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Category>> Create([FromBody] Category category)
        {
            var createdCategory = await _categoryRepository.AddCategoryAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> Update(int id, [FromBody] Category category)
        {
            var updatedCategory = await _categoryRepository.UpdateCategoryAsync(id, category);
            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryRepository.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
