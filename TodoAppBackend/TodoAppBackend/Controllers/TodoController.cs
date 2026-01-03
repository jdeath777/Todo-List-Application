using Microsoft.AspNetCore.Mvc;
using TodoAppBackend.Models;
using TodoAppBackend.Services;

namespace TodoAppBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly ITodoService _todoService;

    public TodoController(ILogger<TodoController> logger, ITodoService todoService)
    {
        _logger = logger;
        _todoService = todoService;
    }

    // ---------------- GET ALL ----------------
    [HttpGet]
    public IActionResult GetTodos()
    {
        _logger.LogInformation("GET /Todo called");
        var todos = _todoService.GetAll() ?? new List<TodoItem>();
        return Ok(todos);
    }

    // ---------------- ADD ----------------
    [HttpPost]
    public IActionResult AddTodo([FromBody] CreateTodoDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest("Title cannot be empty");

        var added = _todoService.Add(dto.Title);
        return Ok(added);
    }

    // ---------------- DELETE ----------------
    [HttpDelete("{id}")]
    public IActionResult DeleteTodo(Guid id)
    {
        var todo = _todoService.GetAll().FirstOrDefault(t => t.Id == id);
        if (todo == null) return NotFound();

        _todoService.Delete(id);
        return NoContent();
    }

    // ---------------- UPDATE (EDIT) ----------------
    [HttpPut("{id}")]
    public IActionResult UpdateTodo(Guid id, [FromBody] CreateTodoDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest("Title cannot be empty");

        var todo = _todoService.GetAll().FirstOrDefault(t => t.Id == id);
        if (todo == null) return NotFound();

        todo.Title = dto.Title;
        return Ok(todo);
    }

    // ---------------- TOGGLE ----------------
    [HttpPatch("{id}/toggle")]
    public IActionResult ToggleTodo(Guid id)
    {
        var todo = _todoService.GetAll().FirstOrDefault(t => t.Id == id);
        if (todo == null) return NotFound();

        todo.IsCompleted = !todo.IsCompleted;
        return Ok(todo);
    }
}
