using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.TodoItemDTOs;

public class TodoItemDto
{

    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;

    public TodoItemStatus CompletionStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public TodoPriority Priority { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
}

