// Models/TodoItem.cs
namespace TodoAppBackend.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class CreateTodoDto
    {
        public string Title { get; set; }
    }
}
