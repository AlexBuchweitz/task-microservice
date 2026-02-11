using System.ComponentModel.DataAnnotations;

namespace TaskService.Models;

public class CreateTaskRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Status { get; set; }
}
