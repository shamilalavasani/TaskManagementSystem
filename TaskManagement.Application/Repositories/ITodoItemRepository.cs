using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;


namespace TaskManagement.Application.Repositories;

public interface ITodoItemRepository
{
    Task<TodoItem> AddAsync(TodoItem todoItem);
    Task<TodoItem?> GetByIdAsync(Guid id);
    Task<PagedResultDto<TodoItem>> GetAllAsync(TodoQueryParametersDto query);
    Task UpdateAsync(TodoItem todoItem);
    Task DeleteAsync(TodoItem todoItem);
    Task<IEnumerable<TodoItem>> GetOverdueAsync();
    Task<IEnumerable<TodoItem>> GetDueInNext7DaysAsync();
}
