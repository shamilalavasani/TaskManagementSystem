using TaskManagement.Application.DTOs.CategoryDTOs;
using TaskManagement.Application.DTOs.CommonDTOs;
using TaskManagement.Application.DTOs.QueryParametersDTOs;

namespace TaskManagement.Application.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto);
    Task<CategoryDto> GetCategoryByIdAsync(Guid id);
    Task<PagedResultDto<CategoryDto>> GetAllCategoriesAsync(CategoryQueryParametersDto query);
    Task UpdateCategoryAsync(Guid id, UpdateCategoryDto updateDto);
    Task DeleteCategoryAsync(Guid id);
}
