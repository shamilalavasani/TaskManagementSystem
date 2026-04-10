using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.TodoItemDTOs;

public class CreateTodoItemDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public Guid CategoryId { get; set; }
    public TodoPriority Priority { get; set; }
}


