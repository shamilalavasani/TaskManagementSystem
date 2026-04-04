using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Services;

public class TodoItemService : ITodoItemService
{
    private readonly ITodoItemRepository _repository;
    public TodoItemService(ITodoItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<TodoItemDto> CreateTodoItemAsync(CreateTodoItemDto createDto)
    {

        var newItem = new TodoItem(

       createDto.Title,
       createDto.Description,
       createDto.DueDate
   );

        var createdItem = await _repository.AddAsync(newItem);
        return MapToDto(createdItem);


    }

    public async Task DeleteTodoItemAsync(Guid id)
    {
        var deletedItem = await _repository.GetByIdAsync(id);

        if (deletedItem is null)
            throw new KeyNotFoundException("Todo item not found.");
        await _repository.DeleteAsync(deletedItem);


    }

    public async Task<IEnumerable<TodoItemDto>> GetAllTodoItemsAsync()
    {

        var items = await _repository.GetAllAsync();
        return items.Select(MapToDto);

    }

    public async Task<IEnumerable<TodoItemDto>> GetCompletedTodoItemsAsync()
    {

        var items = await _repository.GetByStatusAsync(TodoItemStatus.Completed);
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<TodoItemDto>> GetOverdueTodoItemsAsync()
    {
        var items = await _repository.GetOverdueAsync();
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<TodoItemDto>> GetPendingTodoItemsAsync()
    {
        var items = await _repository.GetByStatusAsync(TodoItemStatus.Pending);
        return items.Select(MapToDto);
    }

    public async Task<TodoItemDto> GetTodoItemByIdAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
            throw new KeyNotFoundException("Todo item not found.");

        return MapToDto(item);
    }

    public async Task<IEnumerable<TodoItemDto>> GetTodoItemsDueInNext7DaysAsync()
    {
        var items = await _repository.GetDueInNext7DaysAsync();
        return items.Select(MapToDto);
    }

    public async Task UpdateTodoItemAsync(Guid id, UpdateTodoItemDto updateDto)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
            throw new KeyNotFoundException("Todo item not found.");

        item.UpdateDetails(
                updateDto.Title,
                updateDto.Description,
                updateDto.DueDate

            );
        item.ChangeStatus(updateDto.CompletionStatus);
        await _repository.UpdateAsync(item);

    }

    private static TodoItemDto MapToDto(TodoItem item)
    {
        return new TodoItemDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            CompletionStatus = item.CompletionStatus,
            CreatedAt = item.CreatedAt,
            DueDate = item.DueDate
        };
    }

    public async Task UpdateStatusTodoItemAsync(Guid id, TodoItemStatus updateStatus)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
            throw new KeyNotFoundException("Todo item not found.");

        item.ChangeStatus(updateStatus);
        await _repository.UpdateAsync(item);

    }

    public async Task<IEnumerable<TodoItemDto>> GetInProgressTodoItemsAsync()
    {
        var items = await _repository.GetByStatusAsync(TodoItemStatus.InProgress);
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<TodoItemDto>> GetCancelledTodoItemsAsync()
    {
        var items = await _repository.GetByStatusAsync(TodoItemStatus.Cancelled);
        return items.Select(MapToDto);
    }
}
