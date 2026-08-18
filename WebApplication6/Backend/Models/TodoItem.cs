namespace WebApplication6.Backend.Models;

public sealed class TodoItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public bool IsComplete { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
