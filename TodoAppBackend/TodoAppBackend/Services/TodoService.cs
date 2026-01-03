using TodoAppBackend.Models;

namespace TodoAppBackend.Services
{
    public class TodoService : ITodoService
    {
        private readonly List<TodoItem> _todo = new();

        public IEnumerable<TodoItem> GetAll() => _todo;

        public TodoItem Add(string title)
        {
            var item = new TodoItem
            {
                Id = Guid.NewGuid(),
                Title = title,
                IsCompleted = false
            };

            _todo.Add(item);
            return item;
        }

        public void Delete(Guid id)
        {
            var item = _todo.FirstOrDefault(t => t.Id == id);
            if (item != null)
            {
                _todo.Remove(item);
            }
        }
    }
}
