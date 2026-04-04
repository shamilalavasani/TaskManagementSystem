using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs;

public class TodoItemDto
{

    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }

    public TodoItemStatus CompletionStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }
}

