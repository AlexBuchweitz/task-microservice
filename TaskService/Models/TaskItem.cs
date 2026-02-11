using System.ComponentModel.DataAnnotations;

namespace TaskService.Models;

public class TaskItem
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";
}
