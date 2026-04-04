using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

using TaskManagement.Infrastructure.Persistence.Context;

namespace TaskManagement.Infrastructure.Repositories;

public class TodoItemRepository : ITodoItemRepository
{
    private readonly AppDbContext _context;
    public TodoItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem> AddAsync(TodoItem todoItem)
    {
        await _context.TodoItems.AddAsync(todoItem);
        await _context.SaveChangesAsync();
        return todoItem;
    }

    public async Task DeleteAsync(TodoItem todoItem)
    {
        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        return await _context.TodoItems.ToListAsync();
    }

    public async Task<TodoItem?> GetByIdAsync(Guid id)
    {
        return await _context.TodoItems.FindAsync(id);
    }

    public async Task<IEnumerable<TodoItem>> GetByStatusAsync(TodoItemStatus status)
    {
        return await _context.TodoItems.
            Where(t => t.CompletionStatus == status)
            .ToListAsync();
    }

    public async Task<IEnumerable<TodoItem>> GetDueInNext7DaysAsync()
    {
        return await _context.TodoItems
             .Where(t => t.DueDate > DateTime.UtcNow && t.DueDate <= DateTime.UtcNow.AddDays(7)
             && t.CompletionStatus != TodoItemStatus.Completed)
             .ToListAsync();
    }

    public async Task<IEnumerable<TodoItem>> GetOverdueAsync()
    {
        return await _context.TodoItems
             .Where(t => t.DueDate < DateTime.UtcNow && t.CompletionStatus != TodoItemStatus.Completed)
             .ToListAsync();
    }

    public async Task UpdateAsync(TodoItem todoItem)
    {
        _context.TodoItems.Update(todoItem);
        await _context.SaveChangesAsync();
    }
}
