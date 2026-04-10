using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.DTOs.CommonDTOs;
using TaskManagement.Application.DTOs.QueryParameters;
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

    public async Task<PagedResultDto<TodoItem>> GetAllAsync(TodoQueryParametersDto query)
    {
        IQueryable<TodoItem> todoQuery = _context.TodoItems.AsNoTracking().AsQueryable();

        // Filtering by Status
        if (query.Status.HasValue)
        {
            todoQuery = todoQuery.Where(x => x.CompletionStatus == query.Status.Value);
        }
        // Filtering by Due Date Range
        if (query.DueAfter.HasValue)
        {
            todoQuery = todoQuery.Where(x => x.DueDate >= query.DueAfter.Value);
        }

        if (query.DueBefore.HasValue)
        {
            todoQuery = todoQuery.Where(x => x.DueDate <= query.DueBefore.Value);
        }

        // Search
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var searchTerm = query.Search.Trim().ToLower();

            todoQuery = todoQuery.Where(x =>
                x.Title.ToLower().Contains(searchTerm) ||
               (x.Description != null && x.Description.ToLower().Contains(searchTerm)));
        }

        // Sorting
        todoQuery = (query.SortBy?.ToLower(), query.SortDirection?.ToLower()) switch
        {
            ("title", "asc") => todoQuery.OrderBy(x => x.Title),
            ("title", "desc") => todoQuery.OrderByDescending(x => x.Title),

            ("duedate", "asc") => todoQuery.OrderBy(x => x.DueDate),
            ("duedate", "desc") => todoQuery.OrderByDescending(x => x.DueDate),

            ("createdat", "asc") => todoQuery.OrderBy(x => x.CreatedAt),
            ("createdat", "desc") => todoQuery.OrderByDescending(x => x.CreatedAt),

            _ => todoQuery.OrderByDescending(x => x.CreatedAt)
        };

        // Pagination
        var totalCount = await todoQuery.CountAsync();
        var items = await todoQuery
           .Skip((query.PageNumber - 1) * query.PageSize)
           .Take(query.PageSize)
           .ToListAsync();
        return new PagedResultDto<TodoItem>
        {
            Items = items,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<TodoItem?> GetByIdAsync(Guid id)
    {
        return await _context.TodoItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }



    public async Task<IEnumerable<TodoItem>> GetDueInNext7DaysAsync()
    {
        return await _context.TodoItems
             .Where(t => t.DueDate > DateTime.UtcNow
                    && t.DueDate <= DateTime.UtcNow.AddDays(7)
                    && t.CompletionStatus != TodoItemStatus.Completed
                    && t.CompletionStatus != TodoItemStatus.Cancelled)
             .ToListAsync();
    }

    public async Task<IEnumerable<TodoItem>> GetOverdueAsync()
    {
        return await _context.TodoItems
          .Where(t => t.DueDate < DateTime.UtcNow
                 && t.CompletionStatus != TodoItemStatus.Completed
                 && t.CompletionStatus != TodoItemStatus.Cancelled)
          .ToListAsync();
    }

    public async Task UpdateAsync(TodoItem todoItem)
    {
        _context.TodoItems.Update(todoItem);
        await _context.SaveChangesAsync();
    }
}
