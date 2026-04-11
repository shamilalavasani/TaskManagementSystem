using TaskManagement.Application.DTOs.CategoryDTOs;
using TaskManagement.Application.DTOs.CommonDTOs;
using TaskManagement.Application.DTOs.QueryParametersDTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Repositories;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Services;

public class CategoryService : ICategoryService

{
    private readonly ICategoryRepository _repository;
    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }
    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto)
    {
        var existingCategory = await _repository.ExistsByNameAsync(createDto.Name);
        if (existingCategory)
            throw new ArgumentException("Category with the same name already exists.");

        var category = new Category(
       createDto.Name,
       createDto.Description);

        var createdCategory = await _repository.AddAsync(category);
        return MapToDto(createdCategory);

    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        var category = await _repository.GetByIdAsync(id);

        if (category is null)
            throw new KeyNotFoundException("Category not found.");
        await _repository.DeleteAsync(category);
    }

    public async Task<PagedResultDto<CategoryDto>> GetAllCategoriesAsync(CategoryQueryParametersDto query)
    {
        var pagedItems = await _repository.GetAllAsync(query);
        return new PagedResultDto<CategoryDto>
        {
            Items = pagedItems.Items.Select(MapToDto),
            PageNumber = pagedItems.PageNumber,
            PageSize = pagedItems.PageSize,
            TotalCount = pagedItems.TotalCount
        };

    }

    public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
            throw new KeyNotFoundException("Category not found.");

        return MapToDto(item);
    }

    public async Task UpdateCategoryAsync(Guid id, UpdateCategoryDto updateDto)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
            throw new KeyNotFoundException("Category not found.");
        var existingCategory = await _repository.ExistsByNameAsync(updateDto.Name, id);
        if (existingCategory)
            throw new ArgumentException("Category with the same name already exists.");

        item.Update(
                updateDto.Name,
                updateDto.Description
                       );
        await _repository.UpdateAsync(item);
    }
    private static CategoryDto MapToDto(Category item)
    {
        return new CategoryDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            CreatedAt = item.CreatedAt
        };
    }
}
