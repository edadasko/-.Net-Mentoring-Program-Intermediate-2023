using Catalog.API.Controllers;
using Catalog.Data.Interfaces;
using Catalog.Models;
using Catalog.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Catalog.Api.Tests
{
    public class CategoriesControllerTests
    {
        [Fact]
        public async Task GetById_ReturnsOkObjectResult_WhenCategoryExists()
        {
            // Arrange
            const int testedId = 1;
            var mockRepository = new Mock<ICategoryRepository>();
            mockRepository.Setup(repo => repo.GetCategoryAsync(testedId))
                .ReturnsAsync(CreateFakeCategory(testedId))
                .Verifiable();

            var controller = new CategoriesController(mockRepository.Object);

            // Act
            var result = await controller.GetById(testedId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<Category>(okResult.Value);
            Assert.Equal(testedId, returnValue.Id);
            mockRepository.Verify();
        }

        [Fact]
        public async Task GetById_ThrowsApiException_WhenCategoryNotFound()
        {
            // Arrange
            const int testedId = 2;
            var mockRepository = new Mock<ICategoryRepository>();
            mockRepository.Setup(repo => repo.GetCategoryAsync(testedId))
                .ReturnsAsync((Category)null)
                .Verifiable();

            var controller = new CategoriesController(mockRepository.Object);

            // Act / Assert
            var apiException = await Assert.ThrowsAsync<ApiException>(() => controller.GetById(testedId));
            Assert.Equal(HttpStatusCode.NotFound, apiException.StatusCode);
            mockRepository.Verify();
        }

        [Fact]
        public async Task GetAll_ReturnsOkObjectResult()
        {
            // Arrange
            const int testedId = 1;
            var mockRepository = new Mock<ICategoryRepository>();
            mockRepository.Setup(repo => repo.GetCategoriesAsync())
                .ReturnsAsync(new List<Category> { CreateFakeCategory(testedId) })
                .Verifiable();

            var controller = new CategoriesController(mockRepository.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IList<Category>>(okResult.Value);
            Assert.Equal(testedId, returnValue.Single().Id);
            mockRepository.Verify();
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var categoryToCreate = CreateFakeCategory();
            var mockRepository = new Mock<ICategoryRepository>();
            mockRepository.Setup(repo => repo.AddCategoryAsync(categoryToCreate))
                .ReturnsAsync(categoryToCreate)
                .Verifiable();

            var controller = new CategoriesController(mockRepository.Object);

            // Act
            var result = await controller.Create(categoryToCreate);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsAssignableFrom<Category>(createdAtActionResult.Value);
            Assert.Equal(categoryToCreate.Id, returnValue.Id);
            mockRepository.Verify();
        }

        [Fact]
        public async Task Update_ReturnsOkObjectResult()
        {
            // Arrange
            var categoryToUpdate = CreateFakeCategory();
            var mockRepository = new Mock<ICategoryRepository>();
            mockRepository.Setup(repo => repo.UpdateCategoryAsync(categoryToUpdate.Id, categoryToUpdate))
                .ReturnsAsync(categoryToUpdate)
                .Verifiable();

            var controller = new CategoriesController(mockRepository.Object);

            // Act
            var result = await controller.Update(categoryToUpdate.Id, categoryToUpdate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<Category>(okResult.Value);
            Assert.Equal(categoryToUpdate.Id, returnValue.Id);
            mockRepository.Verify();
        }

        [Fact]
        public async Task Delete_ReturnsNoContentResult()
        {
            // Arrange
            const int testedId = 1;
            var mockRepository = new Mock<ICategoryRepository>();
            mockRepository.Setup(repo => repo.DeleteCategoryAsync(testedId)).Verifiable();

            var controller = new CategoriesController(mockRepository.Object);

            // Act
            var result = await controller.Delete(testedId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            mockRepository.Verify();
        }

        private static Category CreateFakeCategory(int id = 1) =>
            new() { Id = id, Description = "Test", Name = "Test" };
    }
}