using TodoAppBackend.Services;
using TodoAppBackend.Models;
using Xunit;
using System;
using System.Linq;

namespace TodoAppBackend.Tests.Services
{
    public class TodoServiceTests
    {
        [Fact]
        public void Add_ShouldCreateTodo()
        {
            
            var service = new TodoService();
            var title = "Test Todo";

            
            var todo = service.Add(title);

            
            Assert.NotNull(todo);
            Assert.Equal(title, todo.Title);
            Assert.False(todo.IsCompleted);
        }

        [Fact]
        public void GetAll_ShouldReturnAllTodos()
        {
            var service = new TodoService();
            service.Add("Todo 1");
            service.Add("Todo 2");

            var todos = service.GetAll();

            Assert.Equal(2, todos.Count());
        }

        [Fact]
        public void Delete_ShouldRemoveTodo()
        {
            var service = new TodoService();
            var todo = service.Add("Delete me");

            service.Delete(todo.Id);
            var todos = service.GetAll();

            Assert.Empty(todos);
        }

        [Fact]
        public void Delete_NonExistentId_ShouldNotThrow()
        {
            var service = new TodoService();
            service.Add("Test");

            var exception = Record.Exception(() => service.Delete(Guid.NewGuid()));

            Assert.Null(exception);
        }
    }
}
