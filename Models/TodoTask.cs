namespace TaskManagerAPI.Models;

public class TodoTask
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
}

public record TaskRequest(string NewTask);

public record UpdateTaskRequest(string Name, bool IsCompleted);
