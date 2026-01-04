using TodoAppBackend.Controllers;
using TodoAppBackend.Services;
using TodoAppBackend.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoAppBackend.Tests.Controllers
{
    public class TodoControllerTests
    {
        [Fact]
        public void GetTodos_ShouldReturnOkResultWithTodos()
        {
            // Arrange
            var mockService = new Mock<ITodoService>();
            mockService.Setup(s => s.GetAll()).Returns(new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid(), Title = "Test", IsCompleted = false }
            });

            var mockLogger = new Mock<ILogger<TodoController>>();
            var controller = new TodoController(mockLogger.Object, mockService.Object);

            // Act
            var result = controller.GetTodos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var todos = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(okResult.Value);
            Assert.Single(todos);
        }

        [Fact]
        public void AddTodo_ShouldReturnOkResult()
        {
            // Arrange
            var mockService = new Mock<ITodoService>();
            mockService.Setup(s => s.Add(It.IsAny<string>()))
                       .Returns((string title) => new TodoItem
                       {
                           Id = Guid.NewGuid(),
                           Title = title,
                           IsCompleted = false
                       });

            var mockLogger = new Mock<ILogger<TodoController>>();
            var controller = new TodoController(mockLogger.Object, mockService.Object);

            var dto = new CreateTodoDto { Title = "New Todo" };

            // Act
            var result = controller.AddTodo(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var todo = Assert.IsType<TodoItem>(okResult.Value);
            Assert.Equal(dto.Title, todo.Title); // Now passes
        }

    }
}
