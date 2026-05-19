// Endpoints/TaskEndpoints.cs
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/tasks", async (AppDbContext db) =>
        {
            var allTasks = await db.Tasks.ToListAsync();
            return Results.Ok(allTasks);
        });

        app.MapPost("/tasks", async (TaskRequest request, AppDbContext db) =>
        {
            if (request == null || string.IsNullOrWhiteSpace(request.NewTask))
            {
                return Results.BadRequest("Task name cannot be empty");
            }
            
            var newTask = new TodoTask { Name = request.NewTask };
            db.Tasks.Add(newTask);
            await db.SaveChangesAsync();
            
            return Results.Created($"/tasks/{newTask.Id}", newTask);
        });

        app.MapPut("/tasks/{id}", async (int id, UpdateTaskRequest request, AppDbContext db) =>
        {
            var existingTask = await db.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return Results.NotFound($"Task with ID {id} was not found.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Results.BadRequest("You must provide a valid 'Name' to update the task.");
            }

            existingTask.Name = request.Name;
            existingTask.IsCompleted = request.IsCompleted;

            await db.SaveChangesAsync();
            return Results.Ok(new { Message = "Task updated successfully in MySQL!", Task = existingTask });
        });

        app.MapDelete("/tasks/{id}", async (int id, AppDbContext db) =>
        {
            var existingTask = await db.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return Results.NotFound(new { Message = $"Task with ID {id} was not found." });
            }

            db.Tasks.Remove(existingTask);
            await db.SaveChangesAsync();
            
            return Results.Ok(new { Message = $"Task with ID {id} has been successfully deleted from MySQL." });
        });
    }
}