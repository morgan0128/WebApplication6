using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Data;
using WebApplication6.Models;

namespace WebApplication6.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TodosController(ApplicationDbContext dbContext, IWebHostEnvironment environment) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TodoItemDto>>> GetTodos()
    {
        // await EnsureDevelopmentDatabaseCreatedAsync();

        var todos = await dbContext.TodoItems
            .AsNoTracking()
            .OrderBy(todo => todo.IsComplete)
            .ThenByDescending(todo => todo.CreatedAt)
            .Select(todo => ToDto(todo))
            .ToListAsync();

        return Ok(todos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoItemDto>> GetTodo(int id)
    {
        // await EnsureDevelopmentDatabaseCreatedAsync();

        var todo = await dbContext.TodoItems
            .AsNoTracking()
            .Where(todo => todo.Id == id)
            .Select(todo => ToDto(todo))
            .SingleOrDefaultAsync();

        return todo is null ? NotFound() : Ok(todo);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItemDto>> CreateTodo(CreateTodoItemRequest request)
    {
        // await EnsureDevelopmentDatabaseCreatedAsync();

        var title = request.Title?.Trim();
        if (string.IsNullOrWhiteSpace(title))
        {
            ModelState.AddModelError(nameof(request.Title), "A title is required.");
            return ValidationProblem(ModelState);
        }

        var todo = new TodoItem { Title = title };
        dbContext.TodoItems.Add(todo);
        await dbContext.SaveChangesAsync();

        var dto = ToDto(todo);
        return CreatedAtAction(nameof(GetTodo), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TodoItemDto>> UpdateTodo(int id, UpdateTodoItemRequest request)
    {
        // await EnsureDevelopmentDatabaseCreatedAsync();

        var todo = await dbContext.TodoItems.FindAsync(id);
        if (todo is null)
        {
            return NotFound();
        }

        todo.IsComplete = request.IsComplete;
        await dbContext.SaveChangesAsync();

        return Ok(ToDto(todo));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        // await EnsureDevelopmentDatabaseCreatedAsync();

        var todo = await dbContext.TodoItems.FindAsync(id);
        if (todo is null)
        {
            return NotFound();
        }

        dbContext.TodoItems.Remove(todo);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    // private Task EnsureDevelopmentDatabaseCreatedAsync()
    // {
    //     return environment.IsDevelopment()
    //         ? dbContext.Database.EnsureCreatedAsync()
    //         : Task.CompletedTask;
    // }

    private static TodoItemDto ToDto(TodoItem todo)
    {
        return new TodoItemDto(todo.Id, todo.Title, todo.IsComplete, todo.CreatedAt);
    }
}

public sealed record TodoItemDto(int Id, string Title, bool IsComplete, DateTimeOffset CreatedAt);

public sealed record CreateTodoItemRequest(string? Title);

public sealed record UpdateTodoItemRequest(bool IsComplete);
