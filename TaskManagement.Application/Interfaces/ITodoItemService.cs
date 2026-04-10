using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Interfaces;

public interface ITodoItemService
{

    Task<TodoItemDto> CreateTodoItemAsync(CreateTodoItemDto createDto);
    Task<TodoItemDto> GetTodoItemByIdAsync(Guid id);
    Task<PagedResultDto<TodoItemDto>> GetAllTodoItemsAsync(TodoQueryParametersDto query);
    Task UpdateTodoItemAsync(Guid id, UpdateTodoItemDto updateDto);
    Task UpdateStatusTodoItemAsync(Guid id, TodoItemStatus updateStatus);
    Task DeleteTodoItemAsync(Guid id);
    Task<IEnumerable<TodoItemDto>> GetOverdueTodoItemsAsync();

    Task<IEnumerable<TodoItemDto>> GetTodoItemsDueInNext7DaysAsync();

}

