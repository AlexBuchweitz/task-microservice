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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task is null)
            return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskRequest request)
    {
        var title = request.Title.Trim();
        if (title.Length == 0)
        {
            ModelState.AddModelError(nameof(request.Title), "Title must contain at least one non-whitespace character.");
            return ValidationProblem();
        }

        var task = new TaskItem
        {
            Title = title,
            Status = request.Status ?? TaskStatuses.ToDo
        };

        await _repository.AddAsync(task);

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }
}
