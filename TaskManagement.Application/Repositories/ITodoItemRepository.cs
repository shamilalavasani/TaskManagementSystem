using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;


namespace TaskManagement.Application.Repositories;

public interface ITodoItemRepository
{
    Task<TodoItem> AddAsync(TodoItem todoItem);
    Task<TodoItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<TodoItem>> GetAllAsync();
    Task UpdateAsync(TodoItem todoItem);
    Task DeleteAsync(TodoItem todoItem);

    Task<IEnumerable<TodoItem>> GetByStatusAsync(TodoItemStatus status);
    Task<IEnumerable<TodoItem>> GetOverdueAsync();
    Task<IEnumerable<TodoItem>> GetDueInNext7DaysAsync();
}
