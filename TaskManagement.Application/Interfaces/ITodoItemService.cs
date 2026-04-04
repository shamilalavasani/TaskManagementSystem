using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Interfaces;

public interface ITodoItemService
{

    Task<TodoItemDto> CreateTodoItemAsync(CreateTodoItemDto createDto);
    Task<TodoItemDto> GetTodoItemByIdAsync(Guid id);
    Task<IEnumerable<TodoItemDto>> GetAllTodoItemsAsync();
    Task UpdateTodoItemAsync(Guid id, UpdateTodoItemDto updateDto);
    Task UpdateStatusTodoItemAsync(Guid id, TodoItemStatus updateStatus);
    Task DeleteTodoItemAsync(Guid id);
    Task<IEnumerable<TodoItemDto>> GetOverdueTodoItemsAsync();
    Task<IEnumerable<TodoItemDto>> GetCompletedTodoItemsAsync();
    Task<IEnumerable<TodoItemDto>> GetPendingTodoItemsAsync();
    Task<IEnumerable<TodoItemDto>> GetInProgressTodoItemsAsync();
    Task<IEnumerable<TodoItemDto>> GetCancelledTodoItemsAsync();
    Task<IEnumerable<TodoItemDto>> GetTodoItemsDueInNext7DaysAsync();

}

