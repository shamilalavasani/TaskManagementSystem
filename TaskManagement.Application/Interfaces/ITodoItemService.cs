using TaskManagement.Application.DTOs.CommonDTOs;
using TaskManagement.Application.DTOs.QueryParametersDTOs;
using TaskManagement.Application.DTOs.TodoItemDTOs;
using TaskManagement.Domain.Enums;
namespace TaskManagement.Application.Interfaces;

public interface ITodoItemService
{

    Task<TodoItemDto> CreateTodoItemAsync(CreateTodoItemDto createDto, string ownerUserId);
    Task<TodoItemDto> GetTodoItemByIdAsync(Guid id, string userId, bool isAdminOrManager);
    Task<PagedResultDto<TodoItemDto>> GetAllTodoItemsAsync(TodoQueryParametersDto query, string userId, bool isAdminOrManager);
    Task UpdateTodoItemAsync(Guid id, UpdateTodoItemDto updateDto, string userId, bool isAdminOrManager);
    Task UpdateStatusTodoItemAsync(Guid id, TodoItemStatus updateStatus, string userId, bool isAdminOrManager);
    Task DeleteTodoItemAsync(Guid id);
    Task<IEnumerable<TodoItemDto>> GetOverdueTodoItemsAsync();

    Task<IEnumerable<TodoItemDto>> GetTodoItemsDueInNext7DaysAsync();

}

