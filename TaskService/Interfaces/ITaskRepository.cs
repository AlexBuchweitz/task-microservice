using TaskService.Models;

namespace TaskService.Interfaces;

public interface ITaskRepository
{
    Task<TaskItem> AddAsync(TaskItem task);
}
