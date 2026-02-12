using System.ComponentModel.DataAnnotations;

namespace TaskService.Models;

public class TaskItem
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [AllowedValues(TaskStatuses.ToDo, TaskStatuses.InProgress, TaskStatuses.Done)]
    public string Status { get; set; } = TaskStatuses.ToDo;
}
