using TaskManagement.Application.DTOs.CommonDTOs;

using TaskManagement.Application.DTOs.QueryParametersDTOs;
using TaskManagement.Application.DTOs.TodoItemDTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;


namespace TaskManagement.Application.Services;

public class TodoItemService : ITodoItemService
{
    private readonly ITodoItemRepository _repository;
    private readonly ICategoryRepository _categoryRepository;

    public TodoItemService(ITodoItemRepository repository, ICategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
    }

    public async Task<TodoItemDto> CreateTodoItemAsync(CreateTodoItemDto createDto)
    {
        var categoryExists = await _categoryRepository.ExistsAsync(createDto.CategoryId);
        if (!categoryExists)
            throw new KeyNotFoundException("Category not found.");

        var newItem = new TodoItem(

       createDto.Title,
       createDto.Description,
       createDto.DueDate,
       createDto.CategoryId,
       createDto.Priority);

        var createdItem = await _repository.AddAsync(newItem);

        var createdItemWithCategory = await _repository.GetByIdAsync(createdItem.Id);// to fill categoryname 
        if (createdItemWithCategory is null)
            throw new KeyNotFoundException("Created todo item could not be found.");

        return MapToDto(createdItemWithCategory);


    }

    public async Task DeleteTodoItemAsync(Guid id)
    {
        var deletedItem = await _repository.GetByIdAsync(id);

        if (deletedItem is null)
            throw new KeyNotFoundException("Todo item not found.");
        await _repository.DeleteAsync(deletedItem);


    }

    public async Task<PagedResultDto<TodoItemDto>> GetAllTodoItemsAsync(TodoQueryParametersDto query)
    {

        var pagedItems = await _repository.GetAllAsync(query);
        return new PagedResultDto<TodoItemDto>
        {
            Items = pagedItems.Items.Select(MapToDto),
            PageNumber = pagedItems.PageNumber,
            PageSize = pagedItems.PageSize,
            TotalCount = pagedItems.TotalCount
        };

    }



    public async Task<IEnumerable<TodoItemDto>> GetOverdueTodoItemsAsync()
    {
        var items = await _repository.GetOverdueAsync();
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

        var categoryExists = await _categoryRepository.ExistsAsync(updateDto.CategoryId);
        if (!categoryExists)
            throw new KeyNotFoundException("Category not found.");

        item.UpdateDetails(
                updateDto.Title,
                updateDto.Description,
                updateDto.DueDate,
                updateDto.CategoryId,
                updateDto.Priority

            );
        item.ChangeStatus(updateDto.CompletionStatus);
        await _repository.UpdateAsync(item);

    }


    public async Task UpdateStatusTodoItemAsync(Guid id, TodoItemStatus updateStatus)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
            throw new KeyNotFoundException("Todo item not found.");

        item.ChangeStatus(updateStatus);
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
            DueDate = item.DueDate,
            Priority = item.Priority,
            CategoryId = item.CategoryId,
            CategoryName = item.Category?.Name
        };
    }

}
