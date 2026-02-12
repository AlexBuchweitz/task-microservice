using Microsoft.AspNetCore.Mvc;
using TaskService.Interfaces;
using TaskService.Models;

namespace TaskService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository;

    public TasksController(ITaskRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskRequest request)
    {
        var task = new TaskItem
        {
            Title = request.Title,
            Status = request.Status ?? "Pending"
        };

        await _repository.AddAsync(task);

        return CreatedAtAction(nameof(Create), new { id = task.Id }, task);
    }
}
