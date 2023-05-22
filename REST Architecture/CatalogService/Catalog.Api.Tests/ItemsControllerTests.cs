using Catalog.API.Controllers;
using Catalog.Data.Interfaces;
using Catalog.Models;
using Catalog.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Catalog.Api.Tests
{
    public class ItemsControllerTests
    {
        [Fact]
        public async Task GetById_ReturnsOkObjectResult_WhenItemExists()
        {
            // Arrange
            const int testedId = 1;
            var mockRepository = new Mock<IItemRepository>();
            mockRepository.Setup(repo => repo.GetItemAsync(testedId))
                .ReturnsAsync(CreateFakeItem(testedId))
                .Verifiable();

            var controller = new ItemsController(mockRepository.Object);

            // Act
            var result = await controller.GetById(testedId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<Item>(okResult.Value);
            Assert.Equal(testedId, returnValue.Id);
            mockRepository.Verify();
        }

        [Fact]
        public async Task GetById_ThrowsApiException_WhenItemNotFound()
        {
            // Arrange
            const int testedId = 2;
            var mockRepository = new Mock<IItemRepository>();
            mockRepository.Setup(repo => repo.GetItemAsync(testedId))
                .ReturnsAsync((Item)null)
                .Verifiable();

            var controller = new ItemsController(mockRepository.Object);

            // Act / Assert
            var apiException = await Assert.ThrowsAsync<ApiException>(() => controller.GetById(testedId));
            Assert.Equal(HttpStatusCode.NotFound, apiException.StatusCode);
            mockRepository.Verify();
        }

        [Fact]
        public async Task Get_ReturnsOkObjectResult()
        {
            // Arrange
            const int testedId = 1;
            var mockRepository = new Mock<IItemRepository>();
            mockRepository.Setup(repo => repo.GetItemsAsync(1, 1, 10))
                .ReturnsAsync(new List<Item> { CreateFakeItem(testedId) })
                .Verifiable();

            var controller = new ItemsController(mockRepository.Object);

            // Act
            var result = await controller.Get(1, 1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IList<Item>>(okResult.Value);
            Assert.Equal(testedId, returnValue.Single().Id);
            mockRepository.Verify();
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var itemToCreate = CreateFakeItem();
            var mockRepository = new Mock<IItemRepository>();
            mockRepository.Setup(repo => repo.AddItemAsync(itemToCreate))
                .ReturnsAsync(itemToCreate)
                .Verifiable();

            var controller = new ItemsController(mockRepository.Object);

            // Act
            var result = await controller.Create(itemToCreate);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsAssignableFrom<Item>(createdAtActionResult.Value);
            Assert.Equal(itemToCreate.Id, returnValue.Id);
            mockRepository.Verify();
        }

        [Fact]
        public async Task Update_ReturnsOkObjectResult()
        {
            // Arrange
            var itemToUpdate = CreateFakeItem();
            var mockRepository = new Mock<IItemRepository>();
            mockRepository.Setup(repo => repo.UpdateItemAsync(itemToUpdate.Id, itemToUpdate))
                .ReturnsAsync(itemToUpdate)
                .Verifiable();

            var controller = new ItemsController(mockRepository.Object);

            // Act
            var result = await controller.Update(itemToUpdate.Id, itemToUpdate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<Item>(okResult.Value);
            Assert.Equal(itemToUpdate.Id, returnValue.Id);
            mockRepository.Verify();
        }

        [Fact]
        public async Task Delete_ReturnsNoContentResult()
        {
            // Arrange
            const int testedId = 1;
            var mockRepository = new Mock<IItemRepository>();
            mockRepository.Setup(repo => repo.DeleteItemAsync(testedId)).Verifiable();

            var controller = new ItemsController(mockRepository.Object);

            // Act
            var result = await controller.Delete(testedId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            mockRepository.Verify();
        }

        private static Item CreateFakeItem(int id = 1) =>
            new() { Id = id, Description = "Test", Name = "Test", CategoryId = 1 };
    }
}