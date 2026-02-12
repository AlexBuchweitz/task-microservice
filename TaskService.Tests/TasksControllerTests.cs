using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using TaskService.Controllers;
using TaskService.Interfaces;
using TaskService.Models;

namespace TaskService.Tests;

public class TasksControllerTests
{
    private readonly ITaskRepository _repository;
    private readonly TasksController _controller;

    public TasksControllerTests()
    {
        _repository = Substitute.For<ITaskRepository>();
        _controller = new TasksController(_repository);
    }

    [Fact]
    public async Task Create_ValidRequest_Returns201WithTask()
    {
        var request = new CreateTaskRequest { Title = "Test task" };

        _repository.AddAsync(Arg.Any<TaskItem>())
            .Returns(ci => ci.Arg<TaskItem>());

        var result = await _controller.Create(request);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdResult.StatusCode);

        var task = Assert.IsType<TaskItem>(createdResult.Value);
        Assert.Equal("Test task", task.Title);

        await _repository.Received(1).AddAsync(Arg.Is<TaskItem>(t => t.Title == "Test task" && t.Status == TaskStatuses.ToDo));
    }

    [Fact]
    public async Task Create_WithCustomStatus_PreservesStatus()
    {
        var request = new CreateTaskRequest { Title = "Urgent task", Status = TaskStatuses.InProgress };

        _repository.AddAsync(Arg.Any<TaskItem>())
            .Returns(ci => ci.Arg<TaskItem>());

        var result = await _controller.Create(request);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var task = Assert.IsType<TaskItem>(createdResult.Value);
        Assert.Equal(TaskStatuses.InProgress, task.Status);

        await _repository.Received(1).AddAsync(Arg.Is<TaskItem>(t => t.Title == "Urgent task" && t.Status == TaskStatuses.InProgress));
    }

    [Fact]
    public async Task Create_WithNoStatus_DefaultsToToDo()
    {
        var request = new CreateTaskRequest { Title = "Simple task" };

        _repository.AddAsync(Arg.Any<TaskItem>())
            .Returns(ci => ci.Arg<TaskItem>());

        var result = await _controller.Create(request);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var task = Assert.IsType<TaskItem>(createdResult.Value);
        Assert.Equal(TaskStatuses.ToDo, task.Status);

        await _repository.Received(1).AddAsync(Arg.Is<TaskItem>(t => t.Title == "Simple task" && t.Status == TaskStatuses.ToDo));
    }

    [Fact]
    public async Task GetById_ExistingId_Returns200WithTask()
    {
        var expected = new TaskItem { Id = 1, Title = "Test task", Status = TaskStatuses.ToDo };
        _repository.GetByIdAsync(1).Returns(expected);

        var result = await _controller.GetById(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var task = Assert.IsType<TaskItem>(okResult.Value);
        Assert.Equal(1, task.Id);
        Assert.Equal("Test task", task.Title);
        Assert.Equal(TaskStatuses.ToDo, task.Status);
    }

    [Fact]
    public async Task Create_TitleWithWhitespace_IsTrimmed()
    {
        var request = new CreateTaskRequest { Title = "  My Task  " };

        _repository.AddAsync(Arg.Any<TaskItem>())
            .Returns(ci => ci.Arg<TaskItem>());

        var result = await _controller.Create(request);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var task = Assert.IsType<TaskItem>(createdResult.Value);
        Assert.Equal("My Task", task.Title);

        await _repository.Received(1).AddAsync(Arg.Is<TaskItem>(t => t.Title == "My Task"));
    }

    [Theory]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    [InlineData(" \t \n ")]
    public async Task Create_WhitespaceOnlyTitle_ReturnsValidationProblem(string title)
    {
        var request = new CreateTaskRequest { Title = title };

        var result = await _controller.Create(request);

        Assert.IsType<ObjectResult>(result);
        await _repository.DidNotReceive().AddAsync(Arg.Any<TaskItem>());
    }

    [Fact]
    public async Task GetById_NonExistentId_Returns404()
    {
        _repository.GetByIdAsync(999).Returns((TaskItem?)null);

        var result = await _controller.GetById(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
