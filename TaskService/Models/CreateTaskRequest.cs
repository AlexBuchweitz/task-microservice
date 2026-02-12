using System.ComponentModel.DataAnnotations;

namespace TaskService.Models;

public class CreateTaskRequest
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [AllowedValues("To Do", "In Progress", "Done")]
    public string? Status { get; set; }
}
