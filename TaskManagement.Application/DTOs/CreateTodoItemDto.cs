using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Application.DTOs;

public class CreateTodoItemDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
}


