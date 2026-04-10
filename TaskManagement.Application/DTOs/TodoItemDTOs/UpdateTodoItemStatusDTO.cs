using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.TodoItemDTOs;


public class UpdateTodoItemStatusDto
{
    public TodoItemStatus Status { get; set; }
}
