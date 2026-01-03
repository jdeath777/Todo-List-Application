using System;
using TodoAppBackend.Models;
namespace TodoAppBackend.Services
{
	public interface ITodoService
	{
		IEnumerable<TodoItem> GetAll();
		TodoItem Add(string title);
		void Delete(Guid id);

	}
}

