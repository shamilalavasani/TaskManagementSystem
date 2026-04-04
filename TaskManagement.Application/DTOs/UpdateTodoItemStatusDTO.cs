using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs;


public class UpdateTodoItemStatusDto
{
    public TodoItemStatus Status { get; set; }
}
